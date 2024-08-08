namespace LMSweb_v3.ViewModels.StudentManagement
{
    public class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<EnrolledStudent> Students { get; set; }
    }
}
