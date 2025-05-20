using CafeManagementSystemBackend.Data;
using CafeManagementSystemBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystemBackend.Repositories
{
    public class UserRepository : IUserRepository
    
    {
        private readonly CafeDbContext _context;

        public UserRepository(CafeDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
           await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user= await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
           return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);

        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {user.Id} not found.");
            }

            // Update fields
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            if (!string.IsNullOrEmpty(user.PasswordHash))  // Update password only if provided
            {
                existingUser.PasswordHash = user.PasswordHash;
            }

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        }
    }
}
