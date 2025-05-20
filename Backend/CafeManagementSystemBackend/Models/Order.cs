using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagementSystemBackend.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public User Customer { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Completed, etc.
        public ICollection<OrderItem> OrderItems { get; set; }=new List<OrderItem>();
    }
}
