using CafeManagementSystemBackend.DTOs;

using CafeManagementSystemBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CafeManagementSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ILogger<ReservationController> _logger;

        // Constructor that injects IReservationService and ILogger<ReservationController>
        public ReservationController(IReservationService reservationService, ILogger<ReservationController> logger)
        {
            _reservationService = reservationService;
            _logger = logger;
        }

        // Admin and Staff can view all reservations
        [HttpGet]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetAllReservations()
        {
            try
            {
                _logger.LogInformation("Fetching all reservations.");

                var reservations = await _reservationService.GetAllReservationsAsync();

                _logger.LogInformation("Successfully fetched {ReservationCount} reservations.", reservations.Count());

                return Ok(reservations);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while fetching all reservations.");

                return BadRequest(new { message = "An error occurred while fetching reservations." });
            }
        }

        // Admin, Staff, and Customer can view specific reservation
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Staff, Customer")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching reservation details for reservationId: {ReservationId}", id);

                var reservation = await _reservationService.GetReservationByIdAsync(id);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation not found for reservationId: {ReservationId}", id);
                    return NotFound();
                }

                if (User.IsInRole("Customer"))
                {
                    var userIdClaim = User.FindFirst("UserId");

                    if (userIdClaim == null)
                    {
                        _logger.LogWarning("UserId claim is missing in the token for reservationId: {ReservationId}", id);
                        return Unauthorized(new { message = "UserId claim is missing in the token." });
                    }

                    var currentUserId = int.Parse(userIdClaim.Value);

                    if (reservation.CustomerId != currentUserId)
                    {
                        _logger.LogWarning("Unauthorized access attempt by customerId: {CustomerId} for reservationId: {ReservationId}", currentUserId, id);
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = "You are not authorized to access this reservation." });
                    }
                }

                _logger.LogInformation("Successfully fetched reservation details for reservationId: {ReservationId}", id);
                return Ok(reservation);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while fetching reservation details for reservationId: {ReservationId}", id);

                return BadRequest(new { message = "An error occurred while fetching reservation details." });
            }
        }

        // Only Customers can add a reservation
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddReservation([FromBody] AddReservationDTO addReservationDto)
        {
            try
            {
                _logger.LogInformation("Adding a new reservation for customerId: {CustomerId}", addReservationDto.CustomerId);

                var reservation = await _reservationService.AddReservationAsync(addReservationDto);

                _logger.LogInformation("Reservation added successfully with reservationId: {ReservationId}", reservation.Id);

                return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while adding reservation for customerId: {CustomerId}", addReservationDto.CustomerId);

                return BadRequest(new { message = "An error occurred while adding your reservation." });
            }
        }

        // Admin, Staff can delete any reservation; Customers can only delete their own reservations
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Staff, Customer")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                _logger.LogInformation("Deleting reservation for reservationId: {ReservationId}", id);

                var reservation = await _reservationService.GetReservationByIdAsync(id);

                if (reservation == null)
                {
                    _logger.LogWarning("Reservation not found for reservationId: {ReservationId}", id);
                    return NotFound();
                }

                if (User.IsInRole("Customer"))
                {
                    var userIdClaim = User.FindFirst("UserId");

                    if (userIdClaim == null)
                    {
                        _logger.LogWarning("UserId claim is missing in the token for reservationId: {ReservationId}", id);
                        return Unauthorized(new { message = "UserId claim is missing in the token." });
                    }

                    var currentUserId = int.Parse(userIdClaim.Value);

                    if (reservation.CustomerId != currentUserId)
                    {
                        _logger.LogWarning("Unauthorized access attempt by customerId: {CustomerId} for reservationId: {ReservationId}", currentUserId, id);
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = "You are not authorized to delete this reservation." });
                    }
                }

                var success = await _reservationService.DeleteReservationAsync(id);

                if (success)
                {
                    _logger.LogInformation("Reservation with reservationId: {ReservationId} deleted successfully.", id);
                    return NoContent();
                }
                else
                {
                    _logger.LogWarning("Failed to delete reservation with reservationId: {ReservationId}", id);
                    return BadRequest(new { message = "Failed to delete reservation." });
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while deleting reservation for reservationId: {ReservationId}", id);

                return BadRequest(new { message = "An error occurred while deleting the reservation." });
            }
        }



        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetReservationsByCustomerId(int customerId)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsByCustomerIdAsync(customerId);
                if (reservations == null )
                    return NotFound(new { message = "No reservations found for this customer." });

                return Ok(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reservations for customer {CustomerId}", customerId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }


    }
}
