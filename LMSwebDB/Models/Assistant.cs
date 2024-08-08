namespace LMSwebDB.Models;
public class Assistant
{
    public required string AssistantId { get; set; }
    public required string AssistantName { get; set; }

    public virtual ICollection<Manage> Manage { get; set; } = [];
    public virtual User AssistantNavigation { get; set; } = null!;
}