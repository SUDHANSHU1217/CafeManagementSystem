namespace CafeManagementSystemBackend.DTOs
{
    public class AddReservationDTO
    {
        public int CustomerId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int TableNumber { get; set; }
    }
}
