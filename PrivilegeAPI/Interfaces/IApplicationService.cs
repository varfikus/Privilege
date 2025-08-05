using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;

namespace PrivilegeAPI.Interfaces
{
    public interface IApplicationService
    {
        Task<CollectionResult<ApplicationDto>> GetApplicationsAsync();

        Task<BaseResult<ApplicationDto>> GetByIdAsync(int id);

        Task<BaseResult<ApplicationDto>> CreateApplicationAsync(ApplicationDto applicationDto);

        Task<BaseResult<bool>> UpdateApplicationAsync(ApplicationDto applicationDto);
    }
}
