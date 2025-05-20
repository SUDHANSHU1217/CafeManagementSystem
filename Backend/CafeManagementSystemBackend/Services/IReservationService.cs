using CafeManagementSystemBackend.DTOs;

namespace CafeManagementSystemBackend.Services
{
    public interface IReservationService
    {
        Task<List<ReservationDTO>> GetAllReservationsAsync();
        Task<ReservationDTO> GetReservationByIdAsync(int id);
        Task<ReservationDTO> AddReservationAsync(AddReservationDTO addReservationDto);
        Task<bool> DeleteReservationAsync(int id);
        Task<IEnumerable<ReservationDTO>> GetReservationsByCustomerIdAsync(int customerId);
    }
}
