namespace LMSwebDB.Models;

public class Material
{
    public required string MaterialId { get; set; }
    public required string CourseID { get; set; }
    public required string FileName { get; set; }
    public required DateTime UploadTime { get; set; }
    public virtual Course Course { get; set; } = null!;
    public virtual ICollection<QnA> QnAs { get; set; } = null!;
}