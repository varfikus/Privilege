namespace PrivilegeAPI.Models
{
    public class FileLogEvent
    {
        public int Id { get; set; }
        public string File { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string Type { get; set; } = null!; // Giving, Coming, Receiving
    }
}
