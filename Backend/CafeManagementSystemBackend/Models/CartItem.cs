using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagementSystemBackend.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
    }
}
