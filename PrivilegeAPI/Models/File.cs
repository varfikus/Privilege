namespace PrivilegeAPI.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Application? Application { get; set; }
        public DeniedApplication? DeniedApplication { get; set; }
    }
}
