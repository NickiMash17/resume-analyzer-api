using ResumeAnalyzerAPI.Models;

namespace ResumeAnalyzerAPI.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(string email, string password, string fullName);
        Task<User?> AuthenticateAsync(string email, string password);
        string GenerateJwtToken(User user);
    }
} 