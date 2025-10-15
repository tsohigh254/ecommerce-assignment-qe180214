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
public class OrdersController : ControllerBase
{
    private readonly ECommerceDbContext _context;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ECommerceDbContext context, ILogger<OrdersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all orders for the current user
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                OrderDate = o.OrderDate,
                TotalItems = o.TotalItems,
                PaymentIntentId = o.PaymentIntentId,
                PaymentStatus = o.PaymentStatus,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name ?? "",
                    ProductImageUrl = oi.Product?.ImageUrl,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Subtotal = oi.Subtotal
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders");
            return StatusCode(500, new { message = "An error occurred while retrieving orders" });
        }
    }

    /// <summary>
    /// Get a specific order by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            var orderDto = new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                OrderDate = order.OrderDate,
                TotalItems = order.TotalItems,
                PaymentIntentId = order.PaymentIntentId,
                PaymentStatus = order.PaymentStatus,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name ?? "",
                    ProductImageUrl = oi.Product?.ImageUrl,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Subtotal = oi.Subtotal
                }).ToList()
            };

            return Ok(orderDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order {OrderId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the order" });
        }
    }

    /// <summary>
    /// Place an order from the current user's cart (with optional payment)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> PlaceOrder([FromBody] PlaceOrderRequest? request = null)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            // Get user's cart with items
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest(new { message = "Cart is empty. Cannot place order." });
            }

            // Verify all products still exist and have valid prices
            foreach (var cartItem in cart.CartItems)
            {
                if (cartItem.Product == null)
                {
                    return BadRequest(new { message = $"Product with ID {cartItem.ProductId} not found" });
                }
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                TotalAmount = cart.TotalAmount,
                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                PaymentIntentId = request?.PaymentIntentId,
                PaymentStatus = string.IsNullOrEmpty(request?.PaymentIntentId) ? "Pending" : "Processing"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Save to get OrderId

            // Create order items from cart items
            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product!.Price // Store price at time of order
                };
                _context.OrderItems.Add(orderItem);
            }

            // Clear cart items
            _context.CartItems.RemoveRange(cart.CartItems);
            cart.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Fetch the complete order with items
            var createdOrder = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstAsync(o => o.OrderId == order.OrderId);

            var orderDto = new OrderDto
            {
                OrderId = createdOrder.OrderId,
                UserId = createdOrder.UserId,
                TotalAmount = createdOrder.TotalAmount,
                Status = createdOrder.Status,
                OrderDate = createdOrder.OrderDate,
                TotalItems = createdOrder.TotalItems,
                PaymentIntentId = createdOrder.PaymentIntentId,
                PaymentStatus = createdOrder.PaymentStatus,
                Items = createdOrder.OrderItems.Select(oi => new OrderItemDto
                {
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name ?? "",
                    ProductImageUrl = oi.Product?.ImageUrl,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Subtotal = oi.Subtotal
                }).ToList()
            };

            return CreatedAtAction(nameof(GetOrder), new { id = orderDto.OrderId }, orderDto);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error placing order");
            return StatusCode(500, new { message = "An error occurred while placing the order" });
        }
    }

    /// <summary>
    /// Update order status (optional - for admin or processing)
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<ActionResult<OrderDto>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            // Validate status
            var validStatuses = new[] { "Pending", "Processing", "Completed", "Cancelled" };
            if (!validStatuses.Contains(request.Status))
            {
                return BadRequest(new { message = "Invalid status. Valid values: Pending, Processing, Completed, Cancelled" });
            }

            order.Status = request.Status;
            order.UpdatedAt = DateTime.UtcNow;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return await GetOrder(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order status");
            return StatusCode(500, new { message = "An error occurred while updating order status" });
        }
    }

    /// <summary>
    /// Update payment status for an order
    /// </summary>
    [HttpPut("{id}/payment-status")]
    public async Task<ActionResult<OrderDto>> UpdatePaymentStatus(int id, [FromBody] UpdatePaymentStatusRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }

            // Validate payment status
            var validPaymentStatuses = new[] { "Pending", "Processing", "Succeeded", "Failed", "Cancelled" };
            if (!validPaymentStatuses.Contains(request.PaymentStatus))
            {
                return BadRequest(new { message = "Invalid payment status" });
            }

            order.PaymentIntentId = request.PaymentIntentId;
            order.PaymentStatus = request.PaymentStatus;
            order.UpdatedAt = DateTime.UtcNow;

            // Update order status based on payment status
            if (request.PaymentStatus == "Succeeded")
            {
                order.Status = "Processing"; // Order confirmed, ready for processing
            }
            else if (request.PaymentStatus == "Failed" || request.PaymentStatus == "Cancelled")
            {
                order.Status = "Cancelled";
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} payment status updated to {PaymentStatus}", id, request.PaymentStatus);

            return await GetOrder(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment status for order {OrderId}", id);
            return StatusCode(500, new { message = "An error occurred while updating payment status" });
        }
    }
}

// DTOs
public class OrderDto
{
    public int OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public int TotalItems { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public string? PaymentIntentId { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal { get; set; }
}

public class UpdateOrderStatusRequest
{
    public string Status { get; set; } = string.Empty;
}

public class PlaceOrderRequest
{
    public string? PaymentIntentId { get; set; }
}

public class UpdatePaymentStatusRequest
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
}
