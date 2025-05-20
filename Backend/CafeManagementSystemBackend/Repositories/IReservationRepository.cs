using CafeManagementSystemBackend.Models;

namespace CafeManagementSystemBackend.Repositories
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<Reservation> AddReservationAsync(Reservation reservation);
        Task<bool> DeleteReservationAsync(int id);
        Task<IEnumerable<Reservation>> GetReservationsByCustomerIdAsync(int customerId);
    }
}
