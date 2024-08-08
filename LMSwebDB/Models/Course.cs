namespace LMSwebDB.Models;

public class Course
{
    public string CourseId { get; set; } = null!;
    public string? TeacherId { get; set; }
    public string CourseName { get; set; } = null!;
    public string? SystemPrompt { get; set; }
    public string? UserPrompt { get; set; }
    public string? GreetingMessage { get; set; }
    public float Temperature { get; set; }
    public bool IsNeedContext { get; set; }
    public string? LLMModel { get; set; }
    public DateTime CreateTime { get; set; }

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public virtual Teacher? Teacher { get; set; }
    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
    public virtual ICollection<Manage> Manages { get; set; } = new List<Manage>();
}

