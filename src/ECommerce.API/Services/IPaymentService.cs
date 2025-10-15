using ECommerce.Core.Models;

namespace ECommerce.API.Services;

/// <summary>
/// Interface for payment service operations
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Create a payment intent for a specified amount
    /// </summary>
    /// <param name="request">Payment intent creation request</param>
    /// <returns>Payment intent response with client secret</returns>
    Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(CreatePaymentIntentDto request);

    /// <summary>
    /// Confirm a payment intent
    /// </summary>
    /// <param name="paymentIntentId">The Stripe payment intent ID</param>
    /// <returns>Payment status</returns>
    Task<PaymentStatusDto> ConfirmPaymentAsync(string paymentIntentId);

    /// <summary>
    /// Get payment status
    /// </summary>
    /// <param name="paymentIntentId">The Stripe payment intent ID</param>
    /// <returns>Payment status</returns>
    Task<PaymentStatusDto> GetPaymentStatusAsync(string paymentIntentId);

    /// <summary>
    /// Cancel a payment intent
    /// </summary>
    /// <param name="paymentIntentId">The Stripe payment intent ID</param>
    /// <returns>True if cancelled successfully</returns>
    Task<bool> CancelPaymentAsync(string paymentIntentId);

    /// <summary>
    /// Get publishable key for client-side Stripe integration
    /// </summary>
    /// <returns>Stripe publishable key</returns>
    string GetPublishableKey();

    /// <summary>
    /// Handle webhook event from Stripe
    /// </summary>
    /// <param name="json">The raw JSON payload from Stripe</param>
    /// <param name="signature">The Stripe signature header</param>
    /// <returns>True if event was handled successfully</returns>
    Task<bool> HandleWebhookEventAsync(string json, string signature);
}
