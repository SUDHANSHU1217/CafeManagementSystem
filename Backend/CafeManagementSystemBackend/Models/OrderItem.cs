using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagementSystemBackend.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
