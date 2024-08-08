using System.ComponentModel.DataAnnotations;

namespace LMSweb_v3.ViewModels.Course;
 public class CourseDeleteViewModel
{
    [Display(Name = "教師名稱")]
    public string? TeacherName { get; set; }

    [Display(Name = "課程編號")]
    public string? CourseID { get; set; }

    [Display(Name = "課程名稱")]
    public string? CourseName { get; set; }
}