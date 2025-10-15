using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Models;

/// <summary>
/// DTO for creating a payment intent
/// </summary>
public class CreatePaymentIntentDto
{
    [Required]
    public decimal Amount { get; set; }

    public string Currency { get; set; } = "usd";

    public string? Description { get; set; }

    public Dictionary<string, string>? Metadata { get; set; }
}

/// <summary>
/// DTO for payment intent response
/// </summary>
public class PaymentIntentResponseDto
{
    public string PaymentIntentId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string PublishableKey { get; set; } = string.Empty;
}

/// <summary>
/// DTO for confirming a payment
/// </summary>
public class PaymentConfirmDto
{
    [Required]
    public string PaymentIntentId { get; set; } = string.Empty;

    public int? OrderId { get; set; }
}

/// <summary>
/// DTO for payment status
/// </summary>
public class PaymentStatusDto
{
    public string PaymentIntentId { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = string.Empty;

    public bool IsSuccessful { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? CreatedAt { get; set; }
}

/// <summary>
/// DTO for checkout session (combines cart and payment info)
/// </summary>
public class CheckoutSessionDto
{
    public int CartId { get; set; }

    public decimal TotalAmount { get; set; }

    public int TotalItems { get; set; }

    public List<CartItemDto> Items { get; set; } = new();

    public string? PaymentIntentId { get; set; }

    public string? ClientSecret { get; set; }

    public string? PublishableKey { get; set; }
}
