using CafeManagementSystemBackend.Data;
using CafeManagementSystemBackend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CafeManagementSystemBackend.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CafeDbContext _context;

        public CartRepository(CafeDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddCartAsync(Cart cart)
        {
            _context.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<Cart> GetCartByIdAsync(int id)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.MenuItem)
    .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }



        public async Task<CartItem> GetCartItemAsync(int cartId,int menuItemId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.MenuItemId == menuItemId);

        }
        public async Task RemoveCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItem>> GetCartItemsWithMenuItem(int cartId)
        {
            return await _context.CartItems.Where(ci => ci.CartId == cartId)
                .Include(ci => ci.MenuItem)
                .ToListAsync();
        }

        public async Task CreateOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCart(int cartId)
        {
            var cartItem = _context.CartItems.Where(ci => ci.CartId == cartId);
            _context.CartItems.RemoveRange(cartItem);
            await _context.SaveChangesAsync();
        }
        public async Task<Cart> GetCartWithItems(int cartId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }
        public async Task<Cart?> GetCartByCustomerIdAsync(int customerId)
        {
            return await _context.Carts
       .Include(c => c.CartItems)  // Ensure CartItems are loaded
       .ThenInclude(ci=>ci.MenuItem)
       .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }
    }
}
