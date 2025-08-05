using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;
using System.Security.Claims;

namespace PrivilegeAPI.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Генерация токена для доступа к системе
        /// </summary>
        /// <param name="claims">Данные</param>
        /// <returns>Токен</returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// Генерация рефреш-токена
        /// </summary>
        /// <returns>Токен</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Валидация токена. Получить данные из Claims.
        /// </summary>
        /// <param name="accessToken">Токен</param>
        /// <param name="validateLifetime">Проверять валидация времени жизни токена</param>
        /// <returns>Данные пользователя</returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken, bool validateLifetime = true);

        /// <summary>
        /// Обновление токена
        /// </summary>
        /// <param name="dto">Токен</param>
        /// <returns></returns>
        Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto, CancellationToken cancel = default);
    }
}
