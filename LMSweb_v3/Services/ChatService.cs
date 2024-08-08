using ClosedXML.Excel;
using LMSweb_v3.Extensions;
using LMSweb_v3.ViewModels;
using LMSwebDB.Models;
using LMSwebDB.Repositories;

namespace LMSweb_v3.Services;

public class ChatService
{
    private readonly LMSRepository _context;
    private readonly FileOperationService _fileOperation;
    public ChatService(LMSRepository repo, FileOperationService fileOperation)
    {
        _context = repo;
        _fileOperation = fileOperation;
    }

    /// <summary>
    /// 保存歷史紀錄
    /// </summary>
    /// <param name="question">使用者的問題</param>
    /// <param name="generate_answer">GPT產生的答案</param>
    /// <param name="reference_answers">跟使用者問題相關的參考答案</param>
    public async Task SaveLog(QuestionViewModel question, QALogViewModel generate_answer, List<QueryEmbeddingData> reference_answers)
    {
        if (reference_answers.Count > 0)
        {
            foreach (var reference in reference_answers)
            {
                var log = new UserQALog
                {
                    LogId = Guid.NewGuid().ToString(),
                    UserId = question.UserID,
                    UserQuestion = question.Question,
                    CourseId = reference.CourseId,
                    MaterialId = reference.MaterialId,
                    Score = reference.Score,
                    AnswerFromGPT = generate_answer.AnswerFromGPT,
                    SystemPrompt = generate_answer.SystemPrompt,
                    UserPrompt = generate_answer.UserPrompt,
                    PromptToken = generate_answer.PromptUsageInfo.PromptTokens,
                    CompletionToken = generate_answer.PromptUsageInfo.CompletionTokens,
                    TotalToken = generate_answer.PromptUsageInfo.TotalTokens
                };
                _context.Create(log);
                await _context.SaveChangesAsync();
            }
        }
        else
        {
            var log = new UserQALog
            {
                LogId = Guid.NewGuid().ToString(),
                UserId = question.UserID,
                UserQuestion = question.Question,
                CourseId = question.CourseId,
                MaterialId = "",
                Score = 0f,
                AnswerFromGPT = generate_answer.AnswerFromGPT,
                SystemPrompt = generate_answer.SystemPrompt,
                UserPrompt = generate_answer.UserPrompt
            };
            _context.Create(log);
            await _context.SaveChangesAsync();
        }
        
    }

    /// <summary>
    /// Load chat history from JSON
    /// </summary>
    /// <param name="chatHistory">chat history</param>
    /// <returns>chat history</returns>
    public List<ConversationHistory> GetChatHistoryFromJSON(List<string> chatHistory)
    {
        if (chatHistory.Count == 0)
        {
            return [];
        }
        List<ConversationHistory> conversation_history = [];
        for (int i = 0; i < chatHistory.Count/2; i++)
        {
            var item = new ConversationHistory
            {
                Id = i,
                UserQuestion = chatHistory[i],
                AnswerFromGPT = chatHistory[i + 1]
            };
            conversation_history.Add(item);
        }
        return conversation_history;
    }

    /// <summary>
    /// Get inner knowledges
    /// </summary>
    /// <param name="courseId"></param>
    /// <returns>所有內部知識庫的內容</returns>
    public List<InnerKnowledge> GetInnerKnowledges(string courseId)
    {
        var innerKnowkedges = (from qa in _context.Query<QnA>()
                               join m in _context.Query<Material>() on qa.MaterialId equals m.MaterialId
                               join c in _context.Query<Course>() on m.CourseID equals c.CourseId
                               where c.CourseId == courseId
                               select new InnerKnowledge
                               {
                                   CourseId = c.CourseId,
                                   CourseName = c.CourseName,
                                   MaterialId = m.MaterialId,
                                   FileName = m.FileName,
                                   Answer = qa.Answer,
                                   Embeddings = qa.Embeddings
                               }).ToList();
        return innerKnowkedges;
    }

    /// <summary>
    /// 是否需要上下文
    /// </summary>
    /// <param name="courseId">課程編號</param>
    /// <returns>True/False</returns>
    public bool IsNeedContext(string courseId)
    {
        bool isNeedContext = _context.Query<Course>()
                            .Where(x => x.CourseId == courseId)
                            .Select(c => c.IsNeedContext)
                            .SingleOrDefault();
        return isNeedContext;
    }

    public string GetUseModel(string courseId)
    {
        string model = _context.Query<Course>()
                            .Where(x => x.CourseId == courseId)
                            .Select(c => c.LLMModel)
                            .SingleOrDefault() ?? "";
        return model;
    }

    /// <summary>
    /// 取得相似問題的提示語
    /// </summary>
    /// <param name="question">使用者的回應</param>
    /// <returns>相似問題的提示語</returns>
    public string? GetSimilarQuestionsPrompt(string question)
    {
        //經由ChatGPT取得問法不同但語意相同的相似問題
        var promptFilePath = _fileOperation.GetFilePath("sim_question_prompt.txt", "prompt_template");
        if (promptFilePath == null)
        {
            return null;
        }
        var sim_prompt = File.ReadAllText(promptFilePath).Replace("<!sim_question>", question);
        return sim_prompt;
    }

    /// <summary>
    /// 取得提示語
    /// </summary>
    /// <param name="question">使用者的回應</param>
    /// <param name="referenceData">參考資料</param>
    /// <returns>提示語</returns>
    public (string systemPrompt, string userPrompt, float temperature) GetPrompts(QuestionViewModel question, string referenceData)
    {
        string system_prompt = "";
        string user_prompt = "";
        var numOflog = 10;
        var course = _context.Query<Course>()
                            .Where(x => x.CourseId == question.CourseId)
                            .SingleOrDefault();
        var logs =  GetChatHistoryFromJSON(question.History);
        if (course == null) return ("", "", 0f);

        if(!string.IsNullOrEmpty(course.SystemPrompt) && !string.IsNullOrEmpty(course.UserPrompt))
        {
            if (logs.Count > 0)
            {
                logs = logs.TakeLast(numOflog).ToList();
                var history = string.Join("\n", logs.Select(x => $"User: {x.UserQuestion}\nAssistant: {x.AnswerFromGPT.Replace("\n","").Replace(" ","")}\n"));
                var history_prompt = $"\n{history}\n";

                system_prompt = ReplaceKeywords(course.SystemPrompt, history_prompt, question.Question, referenceData);
                user_prompt = ReplaceKeywords(course.UserPrompt, history_prompt, question.Question, referenceData);
                return (system_prompt, user_prompt, course.Temperature);
            }
            system_prompt = ReplaceKeywords(course.SystemPrompt, "", question.Question, referenceData);
            user_prompt = ReplaceKeywords(course.UserPrompt, "", question.Question, referenceData);
            return (system_prompt, user_prompt,course.Temperature);
        }
        return ("", "", 0f);
    }

    /// <summary>
    /// 替換提示語中的關鍵字
    /// </summary>
    /// <param name="template">提示語樣板</param>
    /// <param name="history">對話歷史</param>
    /// <param name="question">使用者回應</param>
    /// <param name="referenceData">參考資料</param>
    /// <returns>替換後的提示語</returns>
    private static string ReplaceKeywords(string template, string history, string question, string referenceData)
    {
        if (template.Contains("<!context>"))
        {
            template = template.Replace("<!context>", history);
        }
        if (template.Contains("<!question>") && !string.IsNullOrEmpty(question))
        {
            template = template.Replace("<!question>", question);
        }
        if (template.Contains("<!reference_data>") && !string.IsNullOrEmpty(referenceData))
        {
            template = template.Replace("<!reference_data>", referenceData);
        }
        return template;
    }

    /// <summary>
    /// 取得歷史對話紀錄
    /// </summary>
    /// <param name="courseId">課程編號</param>
    /// <returns>對話紀錄(CSV格式)</returns>
    public XLWorkbook GetChatHistory(string courseId)
    {
        List<string> headers = ["學號", "姓名", "學生的回應", "GPT的回應","相似度","SystemPrompt", "UserPrompt", "紀錄時間"];
        var chatHistory = (from log in _context.Query<UserQALog>()
                          join s in _context.Query<Student>() on log.UserId equals s.StudentId
                          where log.CourseId == courseId
                          orderby log.UserId, log.CreateTime
                          select new ChatHistoryViewModel
                          {
                              StudentId = s.StudentId,
                              StudentName = s.StudentName,
                              UserQuestion = log.UserQuestion,
                              AnswerFromGPT = log.AnswerFromGPT.Replace("\n", "").Replace(" ", ""),
                              Score = log.Score,
                              SystemPrompt = log.SystemPrompt.Replace("\n", "").Replace(" ", ""),
                              UserPrompt = log.UserPrompt.Replace("\n", "").Replace(" ", ""),
                              CreateTime = log.CreateTime
                          })
                          .ToList();
        return chatHistory.ToExcel(headers);
    }
}
