﻿using System.Security.Principal;

namespace PrivilegeAPI.Models
{
    public class UserToken
    {
        public int Id { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}
