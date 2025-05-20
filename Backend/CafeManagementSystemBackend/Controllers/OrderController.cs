using CafeManagementSystemBackend.Models;
using CafeManagementSystemBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CafeManagementSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        // Constructor that injects IOrderService and ILogger<OrderController>
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder([FromQuery] int cartId)
        {
            try
            {
                // Log an informational message when creating an order
                _logger.LogInformation("Creating order for cartId: {CartId}", cartId);

                var order = await _orderService.CreateOrder(cartId);
                _logger.LogInformation("Order created successfully with OrderId: {OrderId}", order.Id);

                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while creating order for cartId: {CartId}", cartId);
                if(ex.Message == "Cart is empty or does not exist.")
                {
                    return BadRequest(new { message = "Cart is empty." });
                }

                return BadRequest(new { message = "An error occurred while processing your order." });
            }
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Admin, Staff, Customer")]
        public async Task<IActionResult> GetOrdersByCustomerId(int customerId)
        {
            try
            {
                _logger.LogInformation("Fetching orders for customerId: {CustomerId}", customerId);

                var orders = await _orderService.GetOrdersByCustomerId(customerId);

                if (User.IsInRole("Customer"))
                {
                    var userIdClaim = User.FindFirst("UserId");
                    if (userIdClaim == null)
                    {
                        _logger.LogWarning("UserId claim is missing in the token for customerId: {CustomerId}", customerId);
                        return Unauthorized(new { message = "UserId claim is missing in the token." });
                    }

                    var currentUserId = int.Parse(userIdClaim.Value);
                    if (customerId != currentUserId)
                    {
                        _logger.LogWarning("Unauthorized access attempt by customerId: {CustomerId}", customerId);
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = "You are not authorized to access this order." });
                    }
                }

                _logger.LogInformation("Successfully fetched {OrderCount} orders for customerId: {CustomerId}", orders.Count(), customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while fetching orders for customerId: {CustomerId}", customerId);
                return BadRequest(new { message = "An error occurred while fetching orders." });
            }
        }

        [HttpGet("{orderId}")]
        [Authorize(Roles = "Admin, Staff, Customer")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                _logger.LogInformation("Fetching order details for orderId: {OrderId}", orderId);

                var order = await _orderService.GetOrderById(orderId);
                if (order == null)
                {
                    _logger.LogWarning("Order not found for orderId: {OrderId}", orderId);
                    return NotFound();
                }

                if (User.IsInRole("Customer"))
                {
                    var userIdClaim = User.FindFirst("UserId");
                    if (userIdClaim == null)
                    {
                        _logger.LogWarning("UserId claim is missing in the token for orderId: {OrderId}", orderId);
                        return Unauthorized(new { message = "UserId claim is missing in the token." });
                    }

                    var currentUserId = int.Parse(userIdClaim.Value);
                    if (order.CustomerId != currentUserId)
                    {
                        _logger.LogWarning("Unauthorized access attempt by customerId: {CustomerId} for orderId: {OrderId}", currentUserId, orderId);
                        return StatusCode(StatusCodes.Status403Forbidden, new { message = "You are not authorized to access this order." });
                    }
                }

                _logger.LogInformation("Successfully fetched order details for orderId: {OrderId}", orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while fetching order details for orderId: {OrderId}", orderId);
                return BadRequest(new { message = "An error occurred while fetching order details." });
            }
        }
    }
}
