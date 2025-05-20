namespace CafeManagementSystemBackend.DTOs
{
    public class ReservationDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservationDate { get; set; }
        public int TableNumber { get; set; }
    }
}
