namespace CafeManagementSystemBackend.DTOs
{
    public class OrderItemDTO
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
