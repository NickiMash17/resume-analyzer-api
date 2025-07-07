using Microsoft.AspNetCore.Http;

namespace ResumeAnalyzerAPI.Services
{
    public class ResumeParserService : IResumeParser
    {
        public async Task<string> ParseResumeAsync(IFormFile file)
        {
            // TODO: Implement PDF/DOCX parsing
            await Task.Delay(100); // Simulate async work
            return "Extracted resume text (mock)";
        }
    }
} 