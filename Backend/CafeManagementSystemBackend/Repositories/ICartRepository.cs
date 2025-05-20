using CafeManagementSystemBackend.Models;

namespace CafeManagementSystemBackend.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> AddCartAsync(Cart cart);
        Task<Cart> GetCartByIdAsync(int id);
        Task AddCartItemAsync(CartItem cartItem);
        Task SaveChangesAsync();
        Task RemoveCartItemAsync(CartItem cartItem);
        Task<CartItem> GetCartItemAsync(int cartId, int menuItemId);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task<List<CartItem>> GetCartItemsWithMenuItem(int cartId);
           
        Task CreateOrder(Order order);
        Task ClearCart(int cartId);
        Task<Cart> GetCartWithItems(int cartId);
        Task<Cart?> GetCartByCustomerIdAsync(int customerId);
    }
}
