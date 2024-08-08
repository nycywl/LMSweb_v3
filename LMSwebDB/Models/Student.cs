namespace LMSwebDB.Models;

public class Student
{
    public string StudentId { get; set; } = null!;
    public string StudentName { get; set; } = null!;
    public string? Gender { get; set; }

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public virtual User StudentNavigation { get; set; } = null!;
}
