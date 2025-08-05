using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Interfaces;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrivilegeAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _userRepository;
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _lifetime;

        public TokenService(
            ApplicationDbContext userRepository,
            IOptions<JwtSettings> options)
        {
            _userRepository = userRepository;
            _jwtKey = options.Value.SigningKey;
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _lifetime = options.Value.Lifetime;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(_lifetime),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumbers = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumbers);
            return Convert.ToBase64String(randomNumbers);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken, bool validateLifetime = true)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
                ValidateLifetime = validateLifetime,
                ValidAudience = _audience,
                ValidIssuer = _issuer
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("ErrorMessage.InvalidToken");
            return claimsPrincipal;
        }

        public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto, CancellationToken cancel = default)
        {
            try
            {
                var accessToken = dto.AccessToken;
                var refreshToken = dto.RefreshToken;

                // т.к. обновляется просроченный токен, то убрана проверка времени действия
                var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken, false);
                var userName = claimsPrincipal.Identity?.Name;

                var user = await _userRepository.Users
                    .Include(x => x.UserToken)
                    .FirstOrDefaultAsync(x => x.Login == userName, cancel)
                    .ConfigureAwait(false);

                if (user == null ||
                    user.UserToken.RefreshToken != refreshToken ||
                    user.UserToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return new BaseResult<TokenDto>
                    {
                        ErrorMessage = "ErrorMessage.InvalidClientRequest"
                    };
                }

                var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
                var newRefreshToken = GenerateRefreshToken();

                user.UserToken.RefreshToken = newRefreshToken;
                _userRepository.Users.Update(user);

                return new BaseResult<TokenDto>
                {
                    Data = new TokenDto
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<TokenDto>
                {
                    ErrorMessage = "ErrorMessage.InternalServerError",
                    ErrorCode = 1,
                };
            }
        }
    }
}
