using AutoMapper;
using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Helper;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CafeManagementSystemBackend.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<string> RegisterUserAsync(RegisterDTO registerDto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = PasswordHelper.HashPassword(registerDto.Password); // Hash password
            await _userRepository.AddUserAsync(user);

            return "User registered successfully.";
        }

        public async Task<string> LoginUserAsync(LoginDTO loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials.");
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.Name),
                 new Claim(ClaimTypes.Role, user.Role), // Add role
                 new Claim("UserId", user.Id.ToString()), // Add UserId as a custom claim
                 new Claim(ClaimTypes.Email,user.Email)
             };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetUserFromToken(String jwt)
        {
           var jwtt= new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            var email = jwtt.Claims.FirstOrDefault(claim=>claim.Type==ClaimTypes.Email);
            var user = await _userRepository.GetUserByEmailAsync(email?.Value);

            
            return user;
        }
    }
}
