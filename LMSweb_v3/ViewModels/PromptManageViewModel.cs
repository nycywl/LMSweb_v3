using Azure.AI.OpenAI;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMSweb_v3.ViewModels;

public class PromptManageViewModel
{
    public string CourseId { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public string SystemPrompt { get; set; } = null!;
    public string UserPrompt { get; set; } = null!;

    [Required(ErrorMessage = "請輸入招呼語")]
    [DisplayName("招呼語")]
    public string Greeting { get; set; } = null!;

    [DisplayName("創意度")]
    [Range(0.0, 2.0, ErrorMessage = "創意度必須介於 0 到 2 之間")]
    public float Temperature { get; set; }

    [DisplayName("搜尋相關教材時將對話歷程<!context>加入")]
    public bool IsNeedContext { get; set; }

    [DisplayName("選擇模型")]
    public string LLMModel { get; set; } = null!;
}

public class QALogViewModel
{
    public required string SystemPrompt { get; set; }
    public required string UserPrompt { get; set; }
    public required string AnswerFromGPT { get; set; }
    public required CompletionsUsage PromptUsageInfo { get; set; }
}
