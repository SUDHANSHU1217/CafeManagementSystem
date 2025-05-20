using AutoMapper;
using CafeManagementSystemBackend.Data;
using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystemBackend.Services
{
    public class ReservationService:IReservationService
    {
        private readonly CafeDbContext _context;
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationService(CafeDbContext context,IReservationRepository reservationRepository, IMapper mapper)
        { 
            _context = context;
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public async Task<List<ReservationDTO>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllReservationsAsync();
            return _mapper.Map<List<ReservationDTO>>(reservations);
        }

        public async Task<ReservationDTO> GetReservationByIdAsync(int id)
        {
            var reservation = await _reservationRepository.GetReservationByIdAsync(id);
            return _mapper.Map<ReservationDTO>(reservation);
        }

        public async Task<ReservationDTO> AddReservationAsync(AddReservationDTO addReservationDto)
        {
            var reservation = _mapper.Map<Reservation>(addReservationDto);
            var newReservation = await _reservationRepository.AddReservationAsync(reservation);
            var customer = await _context.Users.FindAsync(newReservation.CustomerId);
            if(customer == null)
            {
                throw new Exception("Customer not found");
            }
            var reservationDTO= _mapper.Map<ReservationDTO>(newReservation);
            reservationDTO.CustomerName = customer.Name;
            return reservationDTO;
        }

        public async Task<bool> DeleteReservationAsync(int id)
        {
            return await _reservationRepository.DeleteReservationAsync(id);
        }

        public async Task<IEnumerable<ReservationDTO>> GetReservationsByCustomerIdAsync(int customerId)
        {
            var reservations = await _reservationRepository.GetReservationsByCustomerIdAsync(customerId);

            return reservations.Select(r => new ReservationDTO
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.Name,
                ReservationDate = r.ReservationDate,
                TableNumber=r.TableNumber
            }).ToList();
        }


    }
}
