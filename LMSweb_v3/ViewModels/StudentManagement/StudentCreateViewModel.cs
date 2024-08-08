namespace LMSweb_v3.ViewModels.StudentManagement;

public class StudentCreateViewModel
{
    public string CourseId { get; set; } = null!;
    public string CourseName { get; set; } = null!;
    public EnrolledStudent? Student { get; set; }
}
