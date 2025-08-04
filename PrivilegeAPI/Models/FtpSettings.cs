namespace PrivilegeAPI.Models
{
    public class FtpSettings
    {
        public string Server { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 21;
        public string Username { get; set; } = "admin";
        public string Password { get; set; } = "admin";
    }
}
