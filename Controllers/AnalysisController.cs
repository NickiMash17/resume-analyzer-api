using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeAnalyzerAPI.Services;

namespace ResumeAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    [Authorize]
    public class AnalysisController : ControllerBase
    {
        private readonly IResumeParser _resumeParser;
        private readonly INlpService _nlpService;

        public AnalysisController(IResumeParser resumeParser, INlpService nlpService)
        {
            _resumeParser = resumeParser;
            _nlpService = nlpService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeResume([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var textContent = await _resumeParser.ParseResumeAsync(file);
            var analysisResult = await _nlpService.AnalyzeTextAsync(textContent);

            return Ok(new {
                Success = true,
                Data = analysisResult
            });
        }
    }
} 