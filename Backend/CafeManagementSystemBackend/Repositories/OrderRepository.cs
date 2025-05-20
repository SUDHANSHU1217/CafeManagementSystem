using CafeManagementSystemBackend.Data;
using CafeManagementSystemBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystemBackend.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly CafeDbContext _context;

        public OrderRepository(CafeDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetOrdersByCustomerId(int customerId)
        {
            return await _context.Orders
                                 .Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.MenuItem)
                                 .Where(o => o.CustomerId == customerId)
                                 .ToListAsync();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                                 .Include(o => o.OrderItems)
                                 .ThenInclude(oi => oi.MenuItem)
                                 .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }

}
