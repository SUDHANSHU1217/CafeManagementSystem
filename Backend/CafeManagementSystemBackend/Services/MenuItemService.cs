using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Repositories;

namespace CafeManagementSystemBackend.Services
{
    public class MenuItemService:IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuItemDTO>> GetAllMenuItemsAsync()
        {
            var menuItems = await _menuItemRepository.GetAllMenuItemsAsync();
            return _mapper.Map<IEnumerable<MenuItemDTO>>(menuItems);
        }

        public async Task<MenuItemDTO> GetMenuItemByIdAsync(int id)
        {
            var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(id);
            return _mapper.Map<MenuItemDTO>(menuItem);
        }

        public async Task AddMenuItemAsync(MenuItemDTO menuItemDTO)
        {
            var menuItem = _mapper.Map<MenuItem>(menuItemDTO);
            await _menuItemRepository.AddMenuItemAsync(menuItem);
        }

        public async Task UpdateMenuItemAsync(MenuItemDTO menuItemDTO)
        {
            var menuItem = _mapper.Map<MenuItem>(menuItemDTO);
            await _menuItemRepository.UpdateMenuItemAsync(menuItem);
        }

        public async Task DeleteMenuItemAsync(int id)
        {
            await _menuItemRepository.DeleteMenuItemAsync(id);
        }
    }
}
