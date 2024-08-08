namespace LMSwebDB.Models;

public class QnA
{
    public required string QnAId { get; set; }
    public string Answer { get; set; } = null!;
    public string Embeddings { get; set; } = null!;
    public string CourseId { get; set; } = null!;
    public string MaterialId { get; set; } = null!;
    public virtual Material Material { get; set; } = null!;
}
