using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CafeManagementSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                _logger.LogInformation("Register request received for email: {Email}", registerDto.Email);
                var result = await _authService.RegisterUserAsync(registerDto);
                _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user: {Email}", registerDto.Email);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);
                var token = await _authService.LoginUserAsync(loginDto);
                _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login attempt for email: {Email}", loginDto.Email);
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("jwtuser")]
        public async Task<User> jwtuserlogin()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            // Step 2: Check if the Token Exists
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer"))
            {
                return new User();
            }

            // Step 3: Extract the Token (Remove 'Bearer ' prefix)
            _logger.LogInformation("Login attempt success");
            var token = authHeader.Substring("Bearer ".Length).Trim();
            Console.WriteLine(token);
            return await _authService.GetUserFromToken(token);

        }
    }
}


