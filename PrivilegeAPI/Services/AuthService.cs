using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Helpers;
using PrivilegeAPI.Interfaces;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrivilegeAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IOptions<JwtSettings> _jwtOptions;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, ITokenService tokenService, IOptions<JwtSettings> jwtOptions)
        {
            _context = context;
            _configuration = configuration;
            _tokenService = tokenService;
            _jwtOptions = jwtOptions;
        }
        public async Task<BaseResult<TokenDto>> AuthorizeAsync(AuthDto dto, CancellationToken cancel = default)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Login == dto.Login, cancel)
                    .ConfigureAwait(false);
                if (user is null)
                {
                    return new BaseResult<TokenDto>
                    {
                        ErrorMessage = "error",
                        ErrorCode = 1
                    };
                }

                if (!HashPasswordHelper.IsVerifyPassword(user.Password, dto.Password))
                {
                    return new BaseResult<TokenDto>
                    {
                        ErrorMessage = "error",
                        ErrorCode = 1
                    };
                }

                var userToken = await _context.UserTokens
                    .FirstOrDefaultAsync(x => x.UserId == user.Id, cancel)
                    .ConfigureAwait(false);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login)
                };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                if (userToken is null)
                {
                    userToken = new UserToken
                    {
                        UserId = user.Id,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenValidityInDays)
                    };

                    _context.UserTokens.Add(userToken);
                }
                else
                {
                    userToken.RefreshToken = refreshToken;
                    userToken.RefreshTokenExpiryTime =
                        DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenValidityInDays);

                    _context.UserTokens.Update(userToken);
                }

                return new BaseResult<TokenDto>
                {
                    Data = new TokenDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        UserId = user.Id,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<TokenDto>
                {
                    ErrorMessage = "error",
                    ErrorCode = 1
                };
            }
        }
    }
}
