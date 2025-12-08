using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ResumeAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpPost("upload")]
        public IActionResult Upload([FromForm] TestUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded");
            return Ok("File received");
        }
    }

    public class TestUploadDto
    {
        public IFormFile File { get; set; } = default!;
        public string Description { get; set; } = string.Empty;
    }
} 