namespace LMSweb_v3.Configs;

public class AzureOpenAIConfig
{
    public string Endpoint { get; set; } = null!;
    public string APIKey { get; set; } = null!;
    public string GPTModel { get; set; } = null!;
    public string GPT4Model { get; set; } = null!;
    public string Gpt16kModel { get; set; } = null!;
    public string EmbeddingModel { get; set; } = null!;
}
