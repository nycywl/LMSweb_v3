namespace LMSwebDB.Models
{
    public class StudentCourse
    {
        public string StudentId { get; set; } = null!;
        public string CourseId { get; set; } = null!;

        public virtual Student Student { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
    }
}
