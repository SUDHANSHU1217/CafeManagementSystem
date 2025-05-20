using System.ComponentModel.DataAnnotations.Schema;

namespace CafeManagementSystemBackend.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public User Customer { get; set; }
        public DateTime ReservationDate { get; set; }
        public int TableNumber { get; set; }
    }
}
