namespace PrivilegeAPI.Dto
{
    public class TokenDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int UserId { get; set; }
    }
}