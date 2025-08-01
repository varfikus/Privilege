using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Privilege.API.Services;
using Newtonsoft.Json;
using System.Xml;
using Privilege.API.Models;


namespace Privilege.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiverController : Controller
    {
        [HttpPost]
        public IActionResult ReceiveData([FromBody] string input)
        {
            if (input == null)
                return BadRequest("Invalid input");

            return Ok(new { message = "Файл успешно обработан" });
        }
    }
}