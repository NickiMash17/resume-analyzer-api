using Microsoft.AspNetCore.Http;

namespace ResumeAnalyzerAPI.Models
{
    public class ResumeUploadDto
    {
        public IFormFile File { get; set; } = default!;
        public string Description { get; set; } = string.Empty;
    }
} 