using ResumeAnalyzerAPI.Models;

namespace ResumeAnalyzerAPI.Services
{
    public interface INlpService
    {
        Task<AnalysisResult> AnalyzeTextAsync(string text);
    }
} 