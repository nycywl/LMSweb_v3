using System.ComponentModel.DataAnnotations;

namespace LMSweb_v3.ViewModels;

public class TeacherHomeViewModel
{
    [Display(Name = "課程編號")]
    public string CourseId { get; set; } = "";

    [Display(Name = "課程名稱")]
    public string CourseName { get; set; } = "";
}
