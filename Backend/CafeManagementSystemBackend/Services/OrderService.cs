using AutoMapper;
using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Repositories;

namespace CafeManagementSystemBackend.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<OrderDTO> CreateOrder(int cartId)
        {
            // Get cart details
            var cart = await _cartRepository.GetCartByIdAsync(cartId);
            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty or does not exist.");

            // Create order
            var order = new Order
            {
                CustomerId = cart.CustomerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.MenuItem.Price),
                Status = "Pending",
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    MenuItemId = ci.MenuItemId,
                    Quantity = ci.Quantity,
                    ItemPrice = ci.MenuItem.Price
                }).ToList()
            };

            // Save order
            var createdOrder = await _orderRepository.CreateOrder(order);

            // Map to DTO
            return _mapper.Map<OrderDTO>(createdOrder);
        }

        public async Task<List<OrderDTO>> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerId(customerId);
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            return _mapper.Map<OrderDTO>(order);
        }
    }
}
