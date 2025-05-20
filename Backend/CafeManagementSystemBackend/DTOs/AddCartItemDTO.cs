namespace CafeManagementSystemBackend.DTOs
{
    public class AddCartItemDTO
    {   
        public int CartId { get; set; } 
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }
}
