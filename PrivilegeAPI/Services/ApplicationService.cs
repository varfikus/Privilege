using Microsoft.EntityFrameworkCore;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Interfaces;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;

namespace PrivilegeAPI.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CollectionResult<ApplicationDto>> GetApplicationsAsync()
        {
            var applications = await _context.Applications
                .Select(a => new ApplicationDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Status = a.Status,   
                    DateAdd = a.DateAdd,
                    DateEdit = a.DateEdit,
                    FileId = a.FileId,
                    File = a.File  
                })
                .ToListAsync();

            return new CollectionResult<ApplicationDto>
            {
                Data = applications
            };
        }

        public async Task<BaseResult<ApplicationDto>> GetByIdAsync(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
                return new BaseResult<ApplicationDto>
                {
                    ErrorMessage = "Application not found"
                };

            var dto = new ApplicationDto
            {
                Id = application.Id,
                Name = application.Name,
                Status = application.Status,
                DateAdd = application.DateAdd,
                DateEdit = application.DateEdit,
                FileId = application.FileId,
                File = application.File
            };

            return new BaseResult<ApplicationDto>
            {
                Data = dto
            };
        }

        public async Task<BaseResult<ApplicationDto>> CreateApplicationAsync(ApplicationDto applicationDto)
        {
            try
            {
                var application = new Application
                {
                    Name = applicationDto.Name,
                    Status = (StatusEnum)applicationDto.Status,
                    DateAdd = DateTime.Now,
                    DateEdit = DateTime.Now,
                    FileId = (int)applicationDto.FileId
                };

                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                applicationDto.Id = application.Id;
                applicationDto.DateAdd = application.DateAdd;
                applicationDto.DateEdit = application.DateEdit;

                return new BaseResult<ApplicationDto>
                {
                    Data = applicationDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ApplicationDto>
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<BaseResult<ApplicationDto>> UpdateApplicationAsync(ApplicationDto applicationDto)
        {
            try
            {
                var existingApplication = await _context.Applications.FindAsync(applicationDto.Id);

                if (existingApplication == null)
                    return new BaseResult<ApplicationDto>
                    {
                        ErrorMessage = "Application not found"
                    };

                existingApplication.Status = (StatusEnum)applicationDto.Status;
                existingApplication.DateEdit = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new BaseResult<ApplicationDto>
                {
                    Data = applicationDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<ApplicationDto>
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(a => a.Id == id);
        }
    }
}
