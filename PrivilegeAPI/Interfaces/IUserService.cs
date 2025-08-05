using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;

namespace PrivilegeAPI.Interfaces
{
    public interface IUserService
    {
        Task<CollectionResult<UserDto>> GetUsersAsync();
        Task<BaseResult<UserDto>> GetByIdAsync(int id);
        Task<BaseResult<UserDto>> CreateAsync(UserDto dto);
        Task<BaseResult<UserDto>> UpdateAsync(UserDto dto);
        Task<BaseResult<UserDto>> DeleteAsync(UserDto dto);
    }
}
