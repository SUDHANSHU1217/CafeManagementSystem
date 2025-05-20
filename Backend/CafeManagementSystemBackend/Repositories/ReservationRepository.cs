using CafeManagementSystemBackend.Data;
using CafeManagementSystemBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystemBackend.Repositories
{
    public class ReservationRepository:IReservationRepository
    {
            private readonly CafeDbContext _context;

            public ReservationRepository(CafeDbContext context)
            {
                _context = context;
            }

            public async Task<List<Reservation>> GetAllReservationsAsync()
            {
                return await _context.Reservations.Include(r => r.Customer).ToListAsync();
            }

            public async Task<Reservation> GetReservationByIdAsync(int id)
            {
                return await _context.Reservations.Include(r => r.Customer).FirstOrDefaultAsync(r => r.Id == id);
            }

            public async Task<Reservation> AddReservationAsync(Reservation reservation)
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return reservation;
            }

            public async Task<bool> DeleteReservationAsync(int id)
            {
                var reservation = await _context.Reservations.FindAsync(id);
                if (reservation == null) return false;

                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return true;
            }

              public async Task<IEnumerable<Reservation>> GetReservationsByCustomerIdAsync(int customerId)
              {
               return await _context.Reservations.Where(r => r.CustomerId == customerId).Include(r=>r.Customer).ToListAsync();

             }
    }

    
}
