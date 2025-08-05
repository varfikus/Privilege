using System.Security.Cryptography;
using System.Text;

namespace PrivilegeAPI.Helpers
{
    internal static class HashPasswordHelper
    {
        internal static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        internal static bool IsVerifyPassword(string userPasswordHash, string password)
        {
            var hash = HashPassword(password);
            return hash == userPasswordHash;
        }
    }
}
