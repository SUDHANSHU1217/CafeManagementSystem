using CafeManagementSystemBackend.Data;
using CafeManagementSystemBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystemBackend.Repositories
{
    public class MenuItemRepository : IMenuItemRepository

    {
        private readonly CafeDbContext _context;

        public MenuItemRepository(CafeDbContext context)
        {
            _context = context;
        }
        public async Task AddMenuItemAsync(MenuItem menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int id)
        {
           var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<MenuItem> GetMenuItemByIdAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);

        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
        {
            return await _context.MenuItems.ToListAsync();
        }

        public async Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}
