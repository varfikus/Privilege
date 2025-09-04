using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Interfaces;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using PrivilegeAPI.Services;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.Threading.Channels;
using System.Xml;
using System.Xml.Linq;

namespace PrivilegeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeniedApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IApplicationService _applicationService;
        private readonly IHubContext<XmlProcessingHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public DeniedApplicationsController(ApplicationDbContext context, IHubContext<XmlProcessingHub> hubContext, IApplicationService applicationService, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _hubContext = hubContext;
            _applicationService = applicationService;
            _scopeFactory = scopeFactory;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<DeniedApplicationDto>>> GetApplications()
        {
            var result = await _applicationService.GetDeniedApplicationsAsync();
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("id/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<DeniedApplicationDto>>> GetApplicationById(int id)
        {
            var result = await _applicationService.GetDeniedByIdAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<DeniedApplicationDto>>> CreateApplication([FromBody] DeniedApplicationDto applicationDto)
        {
            var result = await _applicationService.CreateApplicationAsync(applicationDto);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult>> UpdateApplication([FromBody] DeniedApplicationDto updatedApplication)
        {
            var result = await _applicationService.UpdateApplicationAsync(updatedApplication);
            if (!result.IsSuccess)
                return BadRequest(result);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Application {updatedApplication.Id} updated");

            return Ok(result);
        }

        [HttpPut("deny")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult>> DenyApplication([FromBody] DeniedApplicationDto deniedApplication)
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<AnswerService>();

            var result = await _applicationService.UpdateApplicationAsync(deniedApplication);
            if (!result.IsSuccess)
                return BadRequest(result);

            var resultS = await service.SendDeny(deniedApplication);
            if (!resultS)
                return BadRequest(resultS);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Application {deniedApplication.Id} denied");

            return Ok(result);
        }
    }
}