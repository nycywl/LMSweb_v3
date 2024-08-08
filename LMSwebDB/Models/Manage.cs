namespace LMSwebDB.Models;

public class Manage
{
    public required string AssistantId { get; set; }
    public required string CourseId { get; set; }
    public virtual Assistant Assistant { get; set; } = null!;
    public virtual Course Course { get; set; } = null!;
}
