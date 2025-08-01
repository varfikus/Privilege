namespace PrivilegeAPI.Models
{
    public class ApplicationDocument
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string FileName { get; set; }
        public string DataUrl { get; set; } 
    }
}
