namespace LMSweb_v3.ViewModels.Course
{
    public class CourseDetailsViewModel
    {
        public string CourseId { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
        public List<StudentViewModel> Students { get; set; } = new List<StudentViewModel>();
    }
}
