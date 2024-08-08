using System.ComponentModel.DataAnnotations;

namespace LMSweb_v3.ViewModels.Course;

public class CourseCreateViewModel
{
    [Display(Name = "課程名稱")]
    [Required]
    public required string Name { get; set; }
}

public record AsisstantInfo
{
    public required string Prompt { get; set; }
    public required string AssistantName { get; set; }
    public required string UserId { get; set; }
}
