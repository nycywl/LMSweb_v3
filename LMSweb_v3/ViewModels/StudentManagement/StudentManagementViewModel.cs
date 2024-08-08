namespace LMSweb_v3.ViewModels.StudentManagement;

public class StudentManagementViewModel
{
    public required string CourseId { get; set; }
    public required string CourseName { get; set; }

    public List<EnrolledStudent> Students { get; set; } = [];

    public bool FirstCreate { get; set; } = false;
}
