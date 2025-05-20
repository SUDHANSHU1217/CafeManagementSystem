using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagementSystemBackend.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public User Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
