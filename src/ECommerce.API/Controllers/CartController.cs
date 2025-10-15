using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.Core.Models;
using System.Security.Claims;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ECommerceDbContext _context;
    private readonly ILogger<CartController> _logger;

    public CartController(ECommerceDbContext context, ILogger<CartController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get current user's cart with all items
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<CartDto>> GetCart()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Create a new cart if one doesn't exist
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartDto = new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemId = ci.CartItemId,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "",
                    ProductPrice = ci.Product?.Price ?? 0,
                    ProductImageUrl = ci.Product?.ImageUrl,
                    Quantity = ci.Quantity,
                    Subtotal = ci.Subtotal
                }).ToList(),
                TotalAmount = cart.TotalAmount,
                TotalItems = cart.TotalItems
            };

            return Ok(cartDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart");
            return StatusCode(500, new { message = "An error occurred while retrieving the cart" });
        }
    }

    /// <summary>
    /// Add a product to cart or update quantity if already exists
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<CartDto>> AddToCart([FromBody] AddToCartRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            // Validate product exists
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // Get or create cart
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Check if product already in cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            
            if (existingItem != null)
            {
                // Update quantity
                existingItem.Quantity += request.Quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                // Add new item
                var newItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                _context.CartItems.Add(newItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Return updated cart
            return await GetCart();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding to cart");
            return StatusCode(500, new { message = "An error occurred while adding to cart" });
        }
    }

    /// <summary>
    /// Update quantity of a cart item
    /// </summary>
    [HttpPut("item/{cartItemId}")]
    public async Task<ActionResult<CartDto>> UpdateQuantity(int cartItemId, [FromBody] UpdateQuantityRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.Cart!.UserId == userId);

            if (cartItem == null)
            {
                return NotFound(new { message = "Cart item not found" });
            }

            if (request.Quantity <= 0)
            {
                return BadRequest(new { message = "Quantity must be greater than 0" });
            }

            cartItem.Quantity = request.Quantity;
            cartItem.Cart!.UpdatedAt = DateTime.UtcNow;
            
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return await GetCart();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cart item quantity");
            return StatusCode(500, new { message = "An error occurred while updating cart item" });
        }
    }

    /// <summary>
    /// Remove an item from cart
    /// </summary>
    [HttpDelete("item/{cartItemId}")]
    public async Task<ActionResult<CartDto>> RemoveItem(int cartItemId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.Cart!.UserId == userId);

            if (cartItem == null)
            {
                return NotFound(new { message = "Cart item not found" });
            }

            cartItem.Cart!.UpdatedAt = DateTime.UtcNow;
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return await GetCart();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cart item");
            return StatusCode(500, new { message = "An error occurred while removing cart item" });
        }
    }

    /// <summary>
    /// Clear all items from cart
    /// </summary>
    [HttpDelete("clear")]
    public async Task<ActionResult> ClearCart()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound(new { message = "Cart not found" });
            }

            _context.CartItems.RemoveRange(cart.CartItems);
            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cart cleared successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart");
            return StatusCode(500, new { message = "An error occurred while clearing cart" });
        }
    }
}
