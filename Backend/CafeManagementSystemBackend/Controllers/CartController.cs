using CafeManagementSystemBackend.DTOs;

using CafeManagementSystemBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CafeManagementSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpPost("Create")]
        [Authorize(Roles ="Admin,Staff,Customer")]

        public async Task<ActionResult> CreateCart([FromBody] CartCreationDTO cartCreationDTO)
        {
            try
            {
                if (cartCreationDTO == null)
                {
                    _logger.LogWarning("CreateCart: Received null CartCreationDTO.");
                    return BadRequest(new { message = "Invalid cart data." });
                }

                _logger.LogInformation("CreateCart: Creating cart for customerId {CustomerId}.", cartCreationDTO.CustomerId);

                await _cartService.CreateCartAsync(cartCreationDTO);

                _logger.LogInformation("CreateCart: Cart successfully created for customerId {CustomerId}.", cartCreationDTO.CustomerId);
                return StatusCode(StatusCodes.Status201Created, new { message = "Cart created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateCart: An error occurred while creating cart.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the cart." });
            }
        }



    [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Staff, Customer")]
        public async Task<IActionResult> GetCartById(int id)
        {
            try
            {
                var cart = await _cartService.GetCartByIdAsync(id);

                if (cart == null)
                {
                    return NotFound();
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cart with id {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("add-item")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemDTO addCartitemDTO)
        {
            try
            {
                await _cartService.AddCartItemAsync(addCartitemDTO);
                return Ok("Item added to cart successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding cart item");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-item")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveItemFromCart([FromQuery] int cartid, [FromQuery] int menuItemId)
        {
            try
            {
                var result = await _cartService.RemoveItemFromCart(cartid, menuItemId);

                if (result)
                    return Ok("Item removed from cart.");
                else
                    return NotFound("Item not found in cart.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("update-item")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] AddCartItemDTO addCartItemDto)
        {
            try
            {
                var result = await _cartService.UpdateCartItemQuantityAsync(addCartItemDto);

                if (result)
                    return Ok("Cart item quantity updated successfully.");
                else
                    return NotFound("Cart item not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item quantity");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("total")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCartTotal([FromQuery] int cartId)
        {
            try
            {
                var totalAmount = await _cartService.CalculateTotalAmount(cartId);
                var result = new CartTotalDTO { TotalAmount = totalAmount };
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating cart total");
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("checkout")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Checkout([FromQuery] int cartId)
        {
            try
            {
                var result = await _cartService.Checkout(cartId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during checkout");
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCartByCustomerId(int customerId)
        {
            try
            {
                var cart = await _cartService.GetCartByCustomerIdAsync(customerId);
                if (cart == null)
                {
                    return NotFound();
                }
                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cart for customerId: {CustomerId}", customerId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
