using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeAnalyzerAPI.Services;
using ResumeAnalyzerAPI.Models;

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
        public async Task<IActionResult> AnalyzeResume([FromForm] ResumeUploadDto dto)
        {
            try
            {
                if (dto.File == null || dto.File.Length == 0)
                    return BadRequest(new { Success = false, Error = "No file uploaded" });

                var textContent = await _resumeParser.ParseResumeAsync(dto.File);
                
                if (string.IsNullOrWhiteSpace(textContent))
                    return BadRequest(new { Success = false, Error = "Could not extract text from the uploaded file" });

                var analysisResult = await _nlpService.AnalyzeTextAsync(textContent);

                return Ok(new
                {
                    Success = true,
                    Data = analysisResult,
                    FileName = dto.File.FileName,
                    FileSize = dto.File.Length,
                    ProcessedAt = DateTime.UtcNow
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { Success = false, Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Error = "An error occurred while processing your resume. Please try again." });
            }
        }
    }
} 