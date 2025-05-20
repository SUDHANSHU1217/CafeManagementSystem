using CafeManagementSystemBackend.Models;

namespace CafeManagementSystemBackend.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrder(Order order);
        Task<List<Order>> GetOrdersByCustomerId(int customerId);
        Task<Order> GetOrderById(int orderId);
    }
}
