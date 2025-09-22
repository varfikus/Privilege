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
        private readonly AnswerService _answerService;

        public ApplicationService(ApplicationDbContext context, AnswerService answerService)
        {
            _context = context;
            _answerService = answerService;
        }

        public async Task<CollectionResult<ApplicationDto>> GetApplicationsAsync()
        {
            var applications = await _context.Applications
                .Select(a => new ApplicationDto
                {
                    Id = a.Id,
                    Idgosuslug = a.Idgosuslug,
                    Org = a.Org,
                    Orgout = a.Orgout,
                    Orgnumber = a.Orgnumber,
                    Uslugnumber = a.Uslugnumber,
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
                Idgosuslug = application.Idgosuslug,
                Org = application.Org,
                Orgout = application.Orgout,
                Orgnumber = application.Orgnumber,
                Uslugnumber = application.Uslugnumber,
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
                    Idgosuslug = applicationDto.Idgosuslug,
                    Org = applicationDto.Org,
                    Orgout = applicationDto.Orgout,
                    Orgnumber = applicationDto.Orgnumber,
                    Uslugnumber = applicationDto.Uslugnumber,
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

                if (applicationDto.FileId.HasValue)
                    existingApplication.FileId = applicationDto.FileId.Value;

                if (applicationDto.Status.HasValue)
                    existingApplication.Status = applicationDto.Status.Value;

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

        public async Task<BaseResult<ApplicationDto>> SendToMedApplicationAsync(int id)
        {
            try
            {
                ApplicationDto applicationDto;
                var app = await _context.Applications.FindAsync(id);

                if (app == null)
                    return new BaseResult<ApplicationDto>
                    {
                        ErrorMessage = "Application not found"
                    };

                if (await _answerService.SendToMedAsync(app))
                {
                    app.Status = StatusEnum.Delivered;
                    app.DateEdit = DateTime.Now;
                    await _context.SaveChangesAsync();

                    applicationDto = new ApplicationDto
                    {
                        Id = app.Id,
                        Idgosuslug = app.Idgosuslug,
                        Org = app.Org,
                        Orgout = app.Orgout,
                        Orgnumber = app.Orgnumber,
                        Uslugnumber = app.Uslugnumber,
                        Status = app.Status,
                        DateAdd = app.DateAdd,
                        DateEdit = app.DateEdit,
                        FileId = app.FileId,
                        File = app.File
                    };
                }
                else
                {
                    return new BaseResult<ApplicationDto>
                    {
                        ErrorMessage = "Failed to send application to Med"
                    };
                }

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

        public async Task<CollectionResult<DeniedApplicationDto>> GetDeniedApplicationsAsync()
        {
            var applications = await _context.DeniedApplications
                .Select(a => new DeniedApplicationDto
                {
                    Id = a.Id,
                    Idgosuslug = a.Idgosuslug,
                    Org = a.Org,
                    Orgout = a.Orgout,
                    Orgnumber = a.Orgnumber,
                    Uslugnumber = a.Uslugnumber,
                    DateAdd = a.DateAdd,
                    DateEdit = a.DateEdit,
                    FileId = a.FileId,
                    File = a.File,
                    Status = a.Status
                })
                .ToListAsync();

            return new CollectionResult<DeniedApplicationDto>
            {
                Data = applications
            };
        }

        public async Task<BaseResult<DeniedApplicationDto>> GetDeniedByIdAsync(int id)
        {
            var application = await _context.DeniedApplications.FindAsync(id);

            if (application == null)
                return new BaseResult<DeniedApplicationDto>
                {
                    ErrorMessage = "Application not found"
                };

            var dto = new DeniedApplicationDto
            {
                Id = application.Id,
                Idgosuslug = application.Idgosuslug,
                Org = application.Org,
                Orgout = application.Orgout,
                Orgnumber = application.Orgnumber,
                Uslugnumber = application.Uslugnumber,
                DateAdd = application.DateAdd,
                DateEdit = application.DateEdit,
                FileId = application.FileId,
                File = application.File,
                Status = application.Status
            };

            return new BaseResult<DeniedApplicationDto>
            {
                Data = dto
            };
        }

        public async Task<BaseResult<DeniedApplicationDto>> CreateApplicationAsync(DeniedApplicationDto applicationDto)
        {
            try
            {
                var application = new DeniedApplication
                {
                    Idgosuslug = applicationDto.Idgosuslug,
                    Org = applicationDto.Org,
                    Orgout = applicationDto.Orgout,
                    Orgnumber = applicationDto.Orgnumber,
                    Uslugnumber = applicationDto.Uslugnumber,
                    DateAdd = DateTime.Now,
                    DateEdit = DateTime.Now,
                    FileId = (int)applicationDto.FileId,
                    Status = DenyStatus.Add
                };

                _context.DeniedApplications.Add(application);
                await _context.SaveChangesAsync();

                applicationDto.Id = application.Id;
                applicationDto.DateAdd = application.DateAdd;
                applicationDto.DateEdit = application.DateEdit;

                return new BaseResult<DeniedApplicationDto>
                {
                    Data = applicationDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<DeniedApplicationDto>
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<BaseResult<DeniedApplicationDto>> UpdateApplicationAsync(DeniedApplicationDto applicationDto)
        {
            try
            {
                var existingApplication = await _context.DeniedApplications.FindAsync(applicationDto.Id);

                if (existingApplication == null)
                    return new BaseResult<DeniedApplicationDto>
                    {
                        ErrorMessage = "Application not found"
                    };

                existingApplication.Status = applicationDto.Status;
                existingApplication.DateEdit = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new BaseResult<DeniedApplicationDto>
                {
                    Data = applicationDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResult<DeniedApplicationDto>
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
