namespace CafeManagementSystemBackend.DTOs
{
    public class CheckoutDTO
    {
        public int CartId { get; set; }
        public string Status { get; set; } = "Completed";
    }
}
