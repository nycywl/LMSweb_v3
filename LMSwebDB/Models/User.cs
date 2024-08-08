namespace LMSwebDB.Models;
public class User
{
    public string UserId { get; set; } = null!;

    public string Upassword { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public virtual Student? Student { get; set; }

    public virtual Teacher? Teacher { get; set; }

    public virtual Assistant? Assistant { get; set; }
}
