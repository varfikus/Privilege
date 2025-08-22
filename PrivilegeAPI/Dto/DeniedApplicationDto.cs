using PrivilegeAPI.Models;
using File = PrivilegeAPI.Models.File;

namespace PrivilegeAPI.Dto
{
    public class DeniedApplicationDto
    {
        public int Id { get; set; }
        public string? Idgosuslug { get; set; }
        public string? Org { get; set; }
        public string? Orgout { get; set; }
        public string? Orgnumber { get; set; }
        public string? Uslugnumber { get; set; }
        public int? FileId { get; set; }
        public DateTime? DateAdd { get; set; }
        public DateTime? DateEdit { get; set; }
        public DenyStatus Status { get; set; }
        public File? File { get; set; }
    }
}
