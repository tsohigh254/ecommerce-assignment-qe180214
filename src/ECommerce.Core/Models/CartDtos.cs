using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Models;

/// <summary>
/// DTO for Cart
/// </summary>
public class CartDto
{
    public int CartId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
}

/// <summary>
/// DTO for Cart Item
/// </summary>
public class CartItemDto
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public string? ProductImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}

/// <summary>
/// Request DTO for adding product to cart
/// </summary>
public class AddToCartRequest
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; } = 1;
}

/// <summary>
/// Request DTO for updating cart item quantity
/// </summary>
public class UpdateQuantityRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}
