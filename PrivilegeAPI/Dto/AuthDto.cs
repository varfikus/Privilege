namespace PrivilegeAPI.Dto
{
    public class AuthDto
    {
        public AuthDto(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }
        public string Password { get; set; }
    }
}
