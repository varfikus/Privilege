using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;

namespace PrivilegeAPI.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResult<TokenDto>> AuthorizeAsync(AuthDto dto, CancellationToken cancellationToken = default);
    }
}
