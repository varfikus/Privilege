using PrivilegeAPI.Models;
using File = PrivilegeAPI.Models.File;

namespace PrivilegeAPI.Dto
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? FileId { get; set; }
        public StatusEnum? Status { get; set; }
        public DateTime? DateAdd { get; set; }
        public DateTime? DateEdit { get; set; }
        public File? File { get; set; }
    }
}
