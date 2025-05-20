using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;

namespace CafeManagementSystemBackend.Services
{
    public interface ICartService

    {
        Task<CartDTO> CreateCartAsync(CartCreationDTO cartCreationDTO);
        Task<CartDTO> GetCartByIdAsync(int id);
        Task AddCartItemAsync(AddCartItemDTO addCartItemDTO);
        Task<bool> RemoveItemFromCart(int cartId, int menuItemId);
        
        Task<bool> UpdateCartItemQuantityAsync(AddCartItemDTO addCartItemDto);
        
        Task<decimal> CalculateTotalAmount(int cartId);
        Task<CheckoutDTO> Checkout(int cartId);
        Task<CartDTO?> GetCartByCustomerIdAsync(int customerId);

    }
}
