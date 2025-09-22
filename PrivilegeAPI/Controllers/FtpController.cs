using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Interfaces;
using PrivilegeAPI.Result;
using PrivilegeAPI.Services;

namespace PrivilegeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FtpController : ControllerBase
    {
        private readonly FtpService _ftpService;
        private readonly PortalService _portalService;

        public FtpController(FtpService ftpService, PortalService portalService)
        {
            _ftpService = ftpService;
            _portalService = portalService;
        }

        [HttpGet("file")]
        public async Task<IActionResult> GetFile([FromQuery] string remotePath)
        {
            var stream = await _ftpService.OpenReadAsync(remotePath);
            if (stream == null)
                return NotFound("Файл не найден на FTP.");

            var fileName = Path.GetFileName(remotePath);

            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            return File(stream, "application/octet-stream");
        }
    }
}
