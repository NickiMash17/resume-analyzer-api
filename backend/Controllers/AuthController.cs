using Microsoft.AspNetCore.Mvc;
using ResumeAnalyzerAPI.Models;
using ResumeAnalyzerAPI.Services;

namespace ResumeAnalyzerAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.FullName))
                return BadRequest("Email, password, and full name are required.");

            var user = await _authService.RegisterAsync(model.Email, model.Password, model.FullName);
            if (user == null)
                return BadRequest("User already exists");
            return Ok(new { Success = true });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("Email and password are required.");

            var user = await _authService.AuthenticateAsync(model.Email, model.Password);
            if (user == null)
                return Unauthorized();
            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token, ExpiresIn = 3600 });
        }
    }

    public class RegisterModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
    }

    public class LoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
} 