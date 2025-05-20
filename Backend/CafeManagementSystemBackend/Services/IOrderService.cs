using CafeManagementSystemBackend.DTOs;

namespace CafeManagementSystemBackend.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrder(int cartId);
        Task<List<OrderDTO>> GetOrdersByCustomerId(int customerId);
        Task<OrderDTO> GetOrderById(int orderId);
    }
}
