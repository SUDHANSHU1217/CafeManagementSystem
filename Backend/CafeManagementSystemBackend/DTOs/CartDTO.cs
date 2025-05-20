namespace CafeManagementSystemBackend.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<CartItemDTO> Items { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
