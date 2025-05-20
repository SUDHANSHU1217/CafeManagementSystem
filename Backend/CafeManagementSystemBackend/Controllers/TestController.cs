using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagementSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok("This is a public endpoint. No authentication required.");
        }

        // Secured endpoint (Authentication required)
        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureEndpoint()
        {
            var userName = User.Identity.Name; // Get the username from JWT
            return Ok($"Hello {userName}, this is a secured endpoint. You are authenticated!");
        }

        // Role-based secured endpoint (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminEndpoint()
        {
            return Ok("This endpoint is only accessible to Admin users.");
        }

        // Role-based secured endpoint (Staff only)
        [Authorize(Roles = "Staff")]
        [HttpGet("staff")]
        public IActionResult StaffEndpoint()
        {
            return Ok("This endpoint is only accessible to Staff users.");
        }

        // Role-based secured endpoint (Customer only)
        [Authorize(Roles = "Customer")]
        [HttpGet("customer")]
        public IActionResult CustomerEndpoint()
        {
            return Ok("This endpoint is only accessible to Customer users.");
        }
    }

}
