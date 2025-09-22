using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;

namespace PrivilegeAPI.Interfaces
{
    public interface IApplicationService
    {
        Task<CollectionResult<ApplicationDto>> GetApplicationsAsync();

        Task<BaseResult<ApplicationDto>> GetByIdAsync(int id);

        Task<BaseResult<ApplicationDto>> CreateApplicationAsync(ApplicationDto applicationDto);

        Task<BaseResult<ApplicationDto>> UpdateApplicationAsync(ApplicationDto applicationDto);

        Task<BaseResult<ApplicationDto>> SendToMedApplicationAsync(int id);


        Task<CollectionResult<DeniedApplicationDto>> GetDeniedApplicationsAsync();

        Task<BaseResult<DeniedApplicationDto>> GetDeniedByIdAsync(int id);

        Task<BaseResult<DeniedApplicationDto>> CreateApplicationAsync(DeniedApplicationDto applicationDto);

        Task<BaseResult<DeniedApplicationDto>> UpdateApplicationAsync(DeniedApplicationDto applicationDto);
    }
}
