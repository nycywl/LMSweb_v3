using System.ComponentModel.DataAnnotations;

namespace LMSweb_v3.ViewModels.StudentManagement;

public class EnrolledStudent
{
    [Display(Name = "學號")]
    public required string StudentId { get; set; }
    [Display(Name = "姓名")]
    public required string StudentName { get; set; }
    [Display(Name = "性別")]
    public required string StudentSex { get; set; }
}
