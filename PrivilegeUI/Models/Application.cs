using PrivilegeAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace PrivilegeUI.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FileId { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime DateAdd { get; set; }
        public DateTime DateEdit { get; set; }
        public File File { get; set; }
    }
}
