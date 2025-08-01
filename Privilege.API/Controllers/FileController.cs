using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Privilege.API.Models;

namespace Privilege.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpPost("submit")]
        public IActionResult SubmitFileInfo([FromBody] FileUploadDto data)
        {
            if (string.IsNullOrWhiteSpace(data.Link))
                return BadRequest("Ссылка обязательна");

            Console.WriteLine($"Получена ссылка: {data.Link}, Id: {data.Id}, Xml length: {data.Xml?.Length}");

            return Ok(new
            {
                message = "Информация о файле получена",
                link = data.Link,
                id = data.Id,
                xmlSnippet = data.Xml?.Substring(0, Math.Min(100, data.Xml.Length)) 
            });
        }
    }
}
