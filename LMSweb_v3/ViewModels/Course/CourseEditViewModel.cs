namespace LMSweb_v3.ViewModels.Course
{
    public class CourseEditViewModel
    {
        public string CourseId { get; set; } = null!;
        public string TeacherId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string SystemPrompt { get; set; } = null!;
        public string UserPrompt { get; set; } = null!;
        public string GreetingMessage { get; set; } = null!;
        public float Temperature { get; set; }
        public bool IsNeedContext { get; set; }
        public string LLMModel { get; set; } = null!;
    }
}
