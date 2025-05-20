using AutoMapper;
using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Repositories;

namespace CafeManagementSystemBackend.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task AddUserAsync(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(UserDTO userDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(userDTO.Id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userDTO.Id} not found.");
            }

            user.Name = userDTO.Name;
            user.Email = userDTO.Email;

            if (!string.IsNullOrEmpty(userDTO.PasswordHash))  // Update password if provided
            {
                user.PasswordHash = userDTO.PasswordHash;
            }

            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }
    }
}
