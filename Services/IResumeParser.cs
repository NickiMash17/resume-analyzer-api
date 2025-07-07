using Microsoft.AspNetCore.Http;

namespace ResumeAnalyzerAPI.Services
{
    public interface IResumeParser
    {
        Task<string> ParseResumeAsync(IFormFile file);
    }
} 