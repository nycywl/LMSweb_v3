namespace LMSwebDB.Models;

public class Teacher
{
    public string TeacherId { get; set; } = null!;

    public string TeacherName { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual User TeacherNavigation { get; set; } = null!;
}
