using LMSweb_v3.ViewModels.StudentManagement;

namespace LMSweb_v3.ViewModels;

public class ProcessingResultViewModel
{
    public IEnumerable<EnrolledStudent> EnrolledStudents { get; set; } = [];
    public string? Message { get; set; }
}
