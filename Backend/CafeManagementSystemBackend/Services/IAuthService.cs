using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;

namespace CafeManagementSystemBackend.Services
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterDTO registerDto);
        Task<string> LoginUserAsync(LoginDTO loginDto);

         Task<User> GetUserFromToken(String jwt);
    }
}
