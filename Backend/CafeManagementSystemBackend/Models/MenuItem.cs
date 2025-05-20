namespace CafeManagementSystemBackend.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; } // e.g., Beverages, Snacks
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ImageUrl { get; set; }
    }
}
