using System.ComponentModel.DataAnnotations;

namespace PrivilegeAPI.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FileId { get; set; }
        public string Status { get; set; }
        public string DateAdd { get; set; }
        public string DateEdit { get; set; }
        public File File { get; set; }
    }
}
