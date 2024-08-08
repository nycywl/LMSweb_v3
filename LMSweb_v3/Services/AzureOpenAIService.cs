using Azure;
using Azure.AI.OpenAI;
using LMSweb_v3.Configs;
using LMSweb_v3.Extensions;
using LMSweb_v3.ViewModels;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace LMSweb_v3.Services;
public class AzureOpenAIService
{
    private readonly AzureOpenAIConfig _config;
    private readonly OpenAIClient _client;
    private readonly ChatService _chatService;

    public AzureOpenAIService(IOptions<AzureOpenAIConfig> config, ChatService chat)
    {
        _config = config.Value;
        _client = new OpenAIClient(new Uri(_config.Endpoint), new AzureKeyCredential(_config.APIKey));
        _chatService = chat;
    }

    /// <summary>
    /// 取得問題的向量
    /// </summary>
    /// <param name="text">使用者提出的問題</param>
    /// <returns>代表問題的向量</returns>
    /// <exception cref="ArgumentException">
    /// 當參數值為空的時候拋出ArgumentException
    /// </exception>
    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("參數值是空的!", nameof(text));
        }

        List<string> text_list = [text];
        var input = new EmbeddingsOptions(_config.EmbeddingModel, text_list);
        var result = await _client.GetEmbeddingsAsync(input);
        return result.Value.Data[0].Embedding.ToArray();
    }

    /// <summary>
    /// 取得相似問題
    /// </summary>
    /// <param name="question">使用者提出的問題</param>
    /// <returns>問法不同但語意相同的問題</returns>
    private async Task<List<string>> GetSimilarQuestionsAsync(string question)
    {
        //經由ChatGPT取得問法不同但語意相同的相似問題
        
        var sim_prompt = _chatService.GetSimilarQuestionsPrompt(question);
        if (sim_prompt == null)
        {
            return [];
        }
        var op = new ChatCompletionsOptions
        {
            DeploymentName = _config.GPTModel,
            Messages = { new ChatRequestUserMessage(sim_prompt) },
        };

        var sim_result = await _client.GetChatCompletionsAsync(op);
        var sim_question_list = sim_result.Value.Choices[0].Message.Content;

        //使用Regex將項目清單的編號移除
        sim_question_list = Regex.Replace(sim_question_list, @"\d+\. ","");
        return [.. sim_question_list.Split("\n")];
    }

    /// <summary>
    /// 透過ChatGPT根據提供的參考資料與問題產生答案
    /// </summary>
    /// <param name="user_question">使用者提出的問題</param>
    /// <param name="reference_data">參考資料</param>
    /// <returns>GPT生成的答案</returns>
    private async Task<QALogViewModel> GetGenerateAnswerFromGPT(QuestionViewModel question, string reference_data = "")
    {
        (string system_prompt, string user_prompt, float temperature) = _chatService.GetPrompts(question, reference_data);
        string model = _chatService.GetUseModel(question.CourseId);
        model = model switch
        {
            "GPT3" => _config.Gpt16kModel,
            "GPT4" => _config.GPT4Model,
            _ => _config.Gpt16kModel,
        };
        var chat_options = new ChatCompletionsOptions
        {
            DeploymentName = model,
            Messages = {
                    new ChatRequestSystemMessage(system_prompt),
                    new ChatRequestUserMessage(user_prompt)
            },
            Temperature = temperature,
        };
        var chat_result = await _client.GetChatCompletionsAsync(chat_options);
        var gpt_response = chat_result.Value.Choices[0].Message.Content;
        var promptUsageInfo = chat_result.Value.Usage;

        return new QALogViewModel
        {
            SystemPrompt = system_prompt.Replace("\n", "").Replace(" ", ""),
            UserPrompt = user_prompt.Replace("\n", "").Replace(" ", ""),
            AnswerFromGPT = gpt_response.Replace("\n", "").Replace(" ", ""),
            PromptUsageInfo = promptUsageInfo
        };
    }

    /// <summary>
    /// 取得答案(主要公開方法)
    /// </summary>
    /// <param name="question">使用者提出的問題</param>
    /// <returns>GPT生成的答案</returns>
    public async Task<string> GetAnswer(QuestionViewModel question)
    {
        string reference_data = "";
        string history = "";        
        var innerKnowledges = _chatService.GetInnerKnowledges(question.CourseId);
        var histories = _chatService.GetChatHistoryFromJSON(question.History);
        bool isNeedContext = _chatService.IsNeedContext(question.CourseId);
        int numOflog = 10;
        if (histories.Count > 0 && isNeedContext)
        {
            histories = histories.TakeLast(numOflog).ToList();
            history = string.Join("\n", histories.Select(x => $"User: {x.UserQuestion}\nAssistant: {x.AnswerFromGPT}\n").ToList());
            history = $"{history}\n{question.Question}";
        }
        else
        {
            history = question.Question;
        }

        List<QueryEmbeddingData> answers = [];
        if (innerKnowledges.Count > 0)
        {
            var sim_question_list = await GetSimilarQuestionsAsync(history);
            sim_question_list = sim_question_list.Where(x => !string.IsNullOrEmpty(x)).ToList();
            foreach (var sim_question in sim_question_list)
            {
                var queryVector = await GetEmbeddingAsync(sim_question);
                var result = Search(queryVector, innerKnowledges);

                //預期回傳1筆資料
                if (result.Count != 0 && !answers.Any(x => x.MaterialName == result[0].MaterialName))
                {
                    answers.Add(result[0]);
                }
            }
            reference_data = string.Join("\n", answers.Select(answer => answer.Answer));
        }
        
        var generate_answer = await GetGenerateAnswerFromGPT(question, reference_data);
        await _chatService.SaveLog(question, generate_answer, answers);
        return generate_answer.AnswerFromGPT;
    }

    /// <summary>
    /// 搜尋與問題向量最相似的知識
    /// </summary>
    /// <param name="queryVector">問題向量</param>
    /// <param name="knowledges">內部知識</param>
    /// <returns>搜尋結果</returns>
    private static List<QueryEmbeddingData> Search(ICollection<float> queryVector, ICollection<InnerKnowledge> knowledges)
    {
        List<QueryEmbeddingData> data = [];
        foreach (var knowledge in knowledges)
        {
            var faqembsplit = knowledge.Embeddings.Split(',').Select(x => float.Parse(x)).ToList();
            var simscore = faqembsplit.CosineSimilarity(queryVector);
            var dt = new QueryEmbeddingData
            {
                CourseId = knowledge.CourseId,
                CourseName = knowledge.CourseName,
                MaterialId = knowledge.MaterialId,
                MaterialName = knowledge.FileName,
                Answer = knowledge.Answer,
                Score = simscore
            };
            if (dt.Score > 0.75) { data.Add(dt); }
        }

        data = [.. data.OrderByDescending(x => x.Score)];
        return data;
    }
}
