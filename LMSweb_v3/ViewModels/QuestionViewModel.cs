using System.Text.Json.Serialization;

namespace LMSweb_v3.ViewModels;

public class QuestionViewModel
{
    [JsonRequired]
    [JsonPropertyName("user_id")]
    public required string UserID { get; set; }

    [JsonRequired]
    [JsonPropertyName("course_id")]
    public required string CourseId { get; set; }

    [JsonRequired]
    [JsonPropertyName("question")]
    public required string Question { get; set; }

    [JsonPropertyName("history")]
    public List<string> History { get; set; } = [];
}

public class ChatResultViewModel
{
    [JsonPropertyName("answer")]
    public required string Answer { get; set; }
}