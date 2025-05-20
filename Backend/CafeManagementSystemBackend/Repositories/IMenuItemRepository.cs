using CafeManagementSystemBackend.Models;

namespace CafeManagementSystemBackend.Repositories
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync();
        Task<MenuItem> GetMenuItemByIdAsync(int id);
        Task AddMenuItemAsync(MenuItem menuItem);
        Task UpdateMenuItemAsync(MenuItem menuItem);
        Task DeleteMenuItemAsync(int id);
        
    }
}
