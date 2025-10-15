using ECommerce.API.Configuration;
using ECommerce.Core.Models;
using Microsoft.Extensions.Options;
using Stripe;

namespace ECommerce.API.Services;

/// <summary>
/// Stripe payment service implementation
/// </summary>
public class StripePaymentService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;
    private readonly ILogger<StripePaymentService> _logger;

    public StripePaymentService(IOptions<StripeSettings> stripeSettings, ILogger<StripePaymentService> logger)
    {
        _stripeSettings = stripeSettings.Value;
        _logger = logger;
        
        // Configure Stripe API key
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    /// <summary>
    /// Create a payment intent
    /// </summary>
    public async Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(CreatePaymentIntentDto request)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.Amount * 100), // Convert to cents
                Currency = request.Currency.ToLower(),
                Description = request.Description,
                Metadata = request.Metadata,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            _logger.LogInformation("Payment intent created: {PaymentIntentId}", paymentIntent.Id);

            return new PaymentIntentResponseDto
            {
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret,
                Amount = request.Amount,
                Currency = request.Currency,
                Status = paymentIntent.Status,
                PublishableKey = _stripeSettings.PublishableKey
            };
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe error creating payment intent");
            throw new Exception($"Payment error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment intent");
            throw;
        }
    }

    /// <summary>
    /// Confirm a payment intent
    /// </summary>
    public async Task<PaymentStatusDto> ConfirmPaymentAsync(string paymentIntentId)
    {
        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId);

            _logger.LogInformation("Payment intent status: {PaymentIntentId} - {Status}", 
                paymentIntent.Id, paymentIntent.Status);

            return MapToPaymentStatusDto(paymentIntent);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe error confirming payment");
            throw new Exception($"Payment confirmation error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming payment");
            throw;
        }
    }

    /// <summary>
    /// Get payment status
    /// </summary>
    public async Task<PaymentStatusDto> GetPaymentStatusAsync(string paymentIntentId)
    {
        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId);

            return MapToPaymentStatusDto(paymentIntent);
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe error getting payment status");
            throw new Exception($"Payment status error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment status");
            throw;
        }
    }

    /// <summary>
    /// Cancel a payment intent
    /// </summary>
    public async Task<bool> CancelPaymentAsync(string paymentIntentId)
    {
        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.CancelAsync(paymentIntentId);

            _logger.LogInformation("Payment intent cancelled: {PaymentIntentId}", paymentIntent.Id);

            return paymentIntent.Status == "canceled";
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe error cancelling payment");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling payment");
            return false;
        }
    }

    /// <summary>
    /// Get publishable key
    /// </summary>
    public string GetPublishableKey()
    {
        return _stripeSettings.PublishableKey;
    }

    /// <summary>
    /// Handle Stripe webhook events
    /// </summary>
    public async Task<bool> HandleWebhookEventAsync(string json, string signature)
    {
        try
        {
            var webhookSecret = _stripeSettings.WebhookSecret;
            
            if (string.IsNullOrEmpty(webhookSecret))
            {
                _logger.LogWarning("Webhook secret not configured. Skipping signature verification.");
                // In development, you might want to process events without verification
                // In production, you should always verify the signature
            }

            Event stripeEvent;
            
            try
            {
                // Verify webhook signature
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signature,
                    webhookSecret,
                    throwOnApiVersionMismatch: false
                );
                
                _logger.LogInformation("Webhook signature verified successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Webhook signature verification failed");
                return false;
            }

            // Handle the event
            _logger.LogInformation("Received Stripe webhook event: {EventType}", stripeEvent.Type);

            // Payment intent succeeded - this is the main event we care about
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    _logger.LogInformation(
                        "Payment succeeded for PaymentIntent: {PaymentIntentId}, Amount: {Amount}", 
                        paymentIntent.Id, 
                        paymentIntent.Amount / 100m);
                    
                    // The actual order update will be handled by the webhook controller
                    // This service just validates the webhook
                    return true;
                }
            }
            // Payment intent payment failed
            else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    _logger.LogWarning(
                        "Payment failed for PaymentIntent: {PaymentIntentId}, Reason: {Reason}", 
                        paymentIntent.Id,
                        paymentIntent.LastPaymentError?.Message ?? "Unknown");
                    return true;
                }
            }
            // Payment intent created
            else if (stripeEvent.Type == Events.PaymentIntentCreated)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    _logger.LogInformation(
                        "Payment intent created: {PaymentIntentId}", 
                        paymentIntent.Id);
                    return true;
                }
            }
            // Other events can be logged but not necessarily processed
            else
            {
                _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling webhook event");
            return false;
        }
    }

    /// <summary>
    /// Map Stripe PaymentIntent to PaymentStatusDto
    /// </summary>
    private PaymentStatusDto MapToPaymentStatusDto(PaymentIntent paymentIntent)
    {
        var isSuccessful = paymentIntent.Status == "succeeded";
        
        return new PaymentStatusDto
        {
            PaymentIntentId = paymentIntent.Id,
            Status = paymentIntent.Status,
            Amount = paymentIntent.Amount / 100m, // Convert back from cents
            Currency = paymentIntent.Currency,
            IsSuccessful = isSuccessful,
            ErrorMessage = paymentIntent.LastPaymentError?.Message,
            CreatedAt = paymentIntent.Created
        };
    }
}
