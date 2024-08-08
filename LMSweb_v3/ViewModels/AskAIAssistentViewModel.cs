namespace LMSweb_v3.ViewModels;

public class AskAIAssistentViewModel
{
    public required string UserId { get; set; }
    public required string CourseId { get; set; }
    public required string CourseName { get; set; }
    public required string Greeting { get; set; }
}

public record ConversationHistory
{
    public required int Id { get; set; }
    public required string UserQuestion { get; set; }
    public required string AnswerFromGPT { get; set; }
    public DateTime CreateTime { get; set; }
}

public record InnerKnowledge
{
    public required string CourseId { get; set; }
    public required string CourseName { get; set; }
    public required string MaterialId { get; set; }
    public required string FileName { get; set; }
    public required string Answer { get; set; }
    public required string Embeddings { get; set; }
}

public class ChatHistoryViewModel
{
    public required string StudentId { get; set; }
    public required string StudentName { get; set; }
    public required string UserQuestion { get; set; }
    public required string AnswerFromGPT { get; set; }
    public float Score { get; set; } = 0f;
    public required string SystemPrompt { get; set; }
    public required string UserPrompt { get; set; }
    public DateTime CreateTime { get; set; }
}
