using AutoMapper;
using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystemBackend.Services
{
    public class CartService :ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<CartDTO> GetCartByIdAsync(int id)
        {
            var cart = await _cartRepository.GetCartByIdAsync(id);
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task AddCartItemAsync(AddCartItemDTO addCartItemDTO)
        {
            var cartItem = _mapper.Map<CartItem>(addCartItemDTO);
            await _cartRepository.AddCartItemAsync(cartItem);
            await _cartRepository.SaveChangesAsync();
        }
        public async Task<bool> RemoveItemFromCart(int cartId, int menuItemId)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(cartId, menuItemId);
            if (cartItem == null)
                return false;

            await _cartRepository.RemoveCartItemAsync(cartItem);
            return true;
        }

        public async Task<bool> UpdateCartItemQuantityAsync(AddCartItemDTO addCartItemDto)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(addCartItemDto.CartId, addCartItemDto.MenuItemId);
            if (cartItem == null)
                return false;

            cartItem.Quantity = addCartItemDto.Quantity; // Update the quantity
            await _cartRepository.UpdateCartItemAsync(cartItem);

            return true;
        }

        public async Task<decimal> CalculateTotalAmount(int cartId)
        {
            
            var cartItems=await _cartRepository.GetCartItemsWithMenuItem(cartId);
            if(!cartItems.Any())
                return 0;

            
            decimal totalAmount = cartItems.Sum(ci => ci.Quantity * ci.MenuItem.Price);

            return totalAmount; // Return the total amount
        }

        public async Task<CheckoutDTO> Checkout(int cartId)
        {
            // Get the cart with items and related menu items
            var cart = await _cartRepository.GetCartWithItems(cartId);
            if (cart == null)
                throw new Exception("Cart not found.");

            if (!cart.CartItems.Any())
                throw new Exception("Cart is empty.");

            // Calculate the total amount
            var totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.MenuItem.Price);

            // Create an Order
            var order = new Order
            {
                CustomerId = cart.CustomerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Completed"
            };

            // Add OrderItems
            foreach (var cartItem in cart.CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    MenuItemId = cartItem.MenuItemId,
                    Quantity = cartItem.Quantity,
                    ItemPrice = cartItem.MenuItem.Price
                });
            }

            // Save the order
            await _cartRepository.CreateOrder(order);

            // Clear the cart
            await _cartRepository.ClearCart(cartId);

            // Return response
            return new CheckoutDTO { CartId = cartId, Status = "Completed" };
        }

        public async Task<CartDTO> CreateCartAsync(CartCreationDTO cartCreationDto)
        {
            var cart = _mapper.Map<Cart>(cartCreationDto);

            cart.CreatedAt = DateTime.UtcNow;

            // Save to the database
            var createdCart = await _cartRepository.AddCartAsync(cart);

            // Map Entity to DTO for response
            return _mapper.Map<CartDTO>(createdCart);
        }
        public async Task<CartDTO?> GetCartByCustomerIdAsync(int customerId)
        {
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            if (cart == null) return null;

            var cartDTO = new CartDTO
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                Items = cart.CartItems.Select(ci => new CartItemDTO
                {
                    Id = ci.Id,
                    MenuItemId = ci.MenuItemId,
                    MenuItemName=ci.MenuItem.Name,

                    Quantity = ci.Quantity,
                    Price=ci.MenuItem.Price
                }).ToList()
            };

            return cartDTO;
        }
    }
}
