using System.ComponentModel.DataAnnotations;

namespace PrivilegeAPI.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Idgosuslug { get; set; }
        public string? Org { get; set; }
        public string? Orgout { get; set; }
        public string? Orgnumber { get; set; }
        public string Uslugnumber { get; set; }
        public int FileId { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime DateAdd { get; set; }
        public DateTime DateEdit { get; set; }
        public File File { get; set; }
    }
}
