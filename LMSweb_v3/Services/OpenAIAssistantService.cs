using Azure;
using Azure.AI.OpenAI.Assistants;
using LMSweb_v3.Configs;
using LMSweb_v3.ViewModels.Course;
using Microsoft.Extensions.Options;

namespace LMSweb_v3.Services;

/// <summary>
/// TODO: OpenAI助手服務
/// </summary>
public class OpenAiAssistantService
{
    private readonly AssistantsClient _assistantsClient;
    private readonly AzureOpenAIConfig _config;

    public OpenAiAssistantService(IOptions<AzureOpenAIConfig> config)
    {
        _config = config.Value;
        _assistantsClient = new AssistantsClient(new Uri(_config.Endpoint), new AzureKeyCredential(_config.APIKey));
    }

    public async Task<Dictionary<string, string>> CreateOpenAiAssistantAsync(AsisstantInfo asisstant)
    {
        var options = new AssistantCreationOptions(_config.GPTModel)
        {
            Name = asisstant.AssistantName,
            Instructions = asisstant.Prompt,
        };
        var assistant = await _assistantsClient.CreateAssistantAsync(options);
        var threadId = await CreateAssistantThreadAsync(asisstant.UserId);
        return new Dictionary<string, string>
        {
            { "assistantId", assistant.Value.Id },
            { "threadId", threadId }
        };
    }

    public async Task<string> SendMessageAsync(string assistantId, string threadId, string question)
    {
        await _assistantsClient.CreateMessageAsync(threadId, MessageRole.User, question);
        var run = await _assistantsClient.CreateRunAsync(threadId, new CreateRunOptions(assistantId));
        var tmp = _assistantsClient.GetRun(threadId, run.Value.Id);
        Response<ThreadRun> runResponse;
        do
        {
           await Task.Delay(TimeSpan.FromMilliseconds(1000));
            runResponse = await _assistantsClient.GetRunAsync(threadId, run.Value.Id);
        } while (runResponse.Value.Status == RunStatus.Queued || runResponse.Value.Status == RunStatus.InProgress);

        var messageResponse = await _assistantsClient.GetMessagesAsync(threadId);

        foreach (var message in messageResponse.Value.Data.Reverse())
        {
            foreach (var content in message.ContentItems)
            {
                if (content is MessageTextContent textContent)
                {
                    if(textContent.Text != question)
                    {
                        return textContent.Text;
                    }
                }
            }
        }
        return string.Empty;
    }

    public void Delete(string assistantId, string threadId)
    {
        _assistantsClient.DeleteAssistant(assistantId);
        _assistantsClient.DeleteThread(threadId);
    }

    private async Task<string> CreateAssistantThreadAsync(string userId)
    {
        var options = new AssistantThreadCreationOptions
        {
            Metadata = new Dictionary<string, string>
            {
                { "userId", userId }
            }
        };
        var thread = await _assistantsClient.CreateThreadAsync(options);
        return thread.Value.Id;
    }
}
