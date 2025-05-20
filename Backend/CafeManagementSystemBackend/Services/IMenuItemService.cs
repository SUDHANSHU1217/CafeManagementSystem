using CafeManagementSystemBackend.DTOs;

namespace CafeManagementSystemBackend.Services
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemDTO>> GetAllMenuItemsAsync();
        Task<MenuItemDTO> GetMenuItemByIdAsync(int id);
        Task AddMenuItemAsync(MenuItemDTO menuItemDTO);
        Task UpdateMenuItemAsync(MenuItemDTO menuItemDTO);
        Task DeleteMenuItemAsync(int id);
    }
}
