namespace LMSwebDB.Models;

public class UserQALog
{
    public required string LogId { get; set; }
    public required string UserId { get; set; }
    public required string CourseId { get; set; }
    public required string MaterialId { get; set; }
    public string UserQuestion { get; set; } = null!;
    public string AnswerFromGPT { get; set; } = null!;
    public float Score { get; set; }
    public string SystemPrompt { get; set; } = null!;
    public string UserPrompt { get; set; } = null!;
    public int PromptToken { get; set; }
    public int CompletionToken { get; set; }
    public int TotalToken { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
}
