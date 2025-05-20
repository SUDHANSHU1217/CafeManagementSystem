using CafeManagementSystemBackend.DTOs;

using CafeManagementSystemBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CafeManagementSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ILogger<MenuItemController> _logger;

        public MenuItemController(IMenuItemService menuItemService, ILogger<MenuItemController> logger)
        {
            _menuItemService = menuItemService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Staff, Customer")]
        public async Task<ActionResult<IEnumerable<MenuItemDTO>>> GetAllMenuItems()
        {
            try
            {
                var menuItems = await _menuItemService.GetAllMenuItemsAsync();
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all menu items");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Staff, Customer")]
        public async Task<ActionResult<MenuItemDTO>> GetMenuItemById(int id)
        {
            try
            {
                var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);

                if (menuItem == null)
                    return NotFound();

                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching menu item with id {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddMenuItem(MenuItemDTO menuItemDTO)
        {
            try
            {
                await _menuItemService.AddMenuItemAsync(menuItemDTO);
                return CreatedAtAction(nameof(GetMenuItemById), new { id = menuItemDTO.Id }, menuItemDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new menu item");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateMenuItem(int id, MenuItemDTO menuItemDTO)
        {
            try
            {
                if (id != menuItemDTO.Id)
                    return BadRequest("Menu item ID mismatch.");

                await _menuItemService.UpdateMenuItemAsync(menuItemDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu item with id {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            try
            {
                await _menuItemService.DeleteMenuItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu item with id {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}