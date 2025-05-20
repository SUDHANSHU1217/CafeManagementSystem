using CafeManagementSystemBackend.Data;
using CafeManagementSystemBackend.DTOs;
using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace CafeManagementSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly CafeDbContext _context;

        public UserController(CafeDbContext context, IUserService userService,ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
           _context= context;
        }

        // Only Admin can view all users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {

            try
            {

                _logger.LogInformation("Fetching all users.");
                var users = await _userService.GetAllUsersAsync();
                _logger.LogInformation("Successfully fetched {UserCount} users.", users.Count());

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all users.");

                return BadRequest(new { message = "An error occurred while fetching users." });
            }
        }

        // Admin and Staff can view a specific user
        // Users can view their own details
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            try
            {
                _logger.LogInformation("Fetching user details for userId: {UserId}", id);
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User not found for userId: {UserId}", id);
                    return NotFound();
                }

                if (User.IsInRole("Customer"))
                {
                    var userIdClaim = User.FindFirst("UserId");

                    if (userIdClaim == null)
                    {
                        _logger.LogWarning("UserId claim is missing in the token for userId: {UserId}", id);
                        return Unauthorized(new { message = "UserId claim is missing in the token." });
                    }

                    var currentUserId = int.Parse(userIdClaim.Value);

                    if (user.Id != currentUserId)
                    {
                        _logger.LogWarning("Unauthorized access attempt by customerId: {CustomerId} for userId: {UserId}", currentUserId, id);
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = "You are not authorized to access this reservation." });
                    }
                }


                _logger.LogInformation("Successfully fetched user details for userId: {UserId}", id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user details for userId: {UserId}", id);

                return BadRequest(new { message = "An error occurred while fetching user details." });
            }
        }

        // Only Admin can add new users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddUser([FromBody] UserDTO userDTO)
        {
            await _userService.AddUserAsync(userDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = userDTO.Id }, userDTO);
        }

        // Admin and Staff can update user details
        // Customers can update their own details
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Staff, Customer")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserDTO userDTO, [FromQuery] string isPasswordChange)
        {
            try
            {
                if (id != userDTO.Id)
                {
                    _logger.LogWarning("User ID mismatch for userId: {UserId} and userDTO.Id: {UserDTOId}", id, userDTO.Id);
                    return BadRequest("User ID mismatch.");
                }

                if (User.IsInRole("Customer"))
                {
                    var userIdClaim = User.FindFirst("UserId");

                    if (userIdClaim == null)
                    {
                        _logger.LogWarning("UserId claim is missing in the token for userId: {UserId}", id);
                        return Unauthorized(new { message = "UserId claim is missing in the token." });
                    }

                    var currentUserId = int.Parse(userIdClaim.Value);

                    if (userDTO.Id != currentUserId)
                    {
                        _logger.LogWarning("Unauthorized access attempt by customerId: {CustomerId} for userId: {UserId}", currentUserId, id);
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = "You are not authorized to access this reservation." });
                    }
                }

                _logger.LogInformation("Updating user details for userId: {UserId}", id);
                await _userService.UpdateUserAsync(userDTO);
                _logger.LogInformation("Successfully updated user details for userId: {UserId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user details for userId: {UserId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating user details." });
            }
        }

        // Only Admin can delete a user
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with userId: {UserId}", id);

                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User not found for userId: {UserId}", id);
                    return NotFound();
                }

                await _userService.DeleteUserAsync(id);
                _logger.LogInformation("Successfully deleted user with userId: {UserId}", id);
                return NoContent();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while deleting user with userId: {UserId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the user." });
            }

        }
        //[HttpGet("sum/{a},{b}")]
        //    public async Task<IActionResult> Sum(int a,int b)
        //{
        //    return Ok(a+b);
        //} 
   
    }
}
