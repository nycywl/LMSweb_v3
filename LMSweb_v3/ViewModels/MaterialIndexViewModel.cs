namespace LMSweb_v3.ViewModels;
public class MaterialIndexViewModel
{
    public required string CourseID { get; set; }
    public required string CourseName { get; set; }
    public List<MaterialItem> Materials { get; set; } = [];
}
public class MaterialItem
{
    public string MaterialID { get; set; }
    public string? FileName { get; set; }
    public string? UploadTime { get; set; }
    public bool IsTrainingMaterial { get; set; }
}