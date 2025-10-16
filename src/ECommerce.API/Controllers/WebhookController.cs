using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.Services;
using Stripe;
using Stripe.Checkout;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebhookController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ECommerceDbContext _context;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(
        IPaymentService paymentService,
        ECommerceDbContext context,
        ILogger<WebhookController> logger)
    {
        _paymentService = paymentService;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Stripe webhook endpoint
    /// This endpoint receives events from Stripe and updates order payment status accordingly
    /// </summary>
    [HttpPost("stripe")]
    public async Task<IActionResult> StripeWebhook()
    {
        try
        {
            // Read the raw request body
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();

            // Get the Stripe signature header
            var signature = Request.Headers["Stripe-Signature"].ToString();

            if (string.IsNullOrEmpty(signature))
            {
                _logger.LogWarning("Webhook received without Stripe-Signature header");
                return BadRequest(new { error = "Missing Stripe signature" });
            }

            // Verify and handle the webhook event
            var eventHandled = await _paymentService.HandleWebhookEventAsync(json, signature);
            
            if (!eventHandled)
            {
                _logger.LogWarning("Webhook event could not be verified or processed");
                return BadRequest(new { error = "Invalid webhook signature" });
            }

            // Parse the event to get the payment intent
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ParseEvent(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse Stripe event");
                return BadRequest(new { error = "Invalid event format" });
            }

            _logger.LogInformation("Processing webhook event: {EventType} - {EventId}", 
                stripeEvent.Type, stripeEvent.Id);

            // Handle different Stripe events
            switch (stripeEvent.Type)
            {
                // Payment completed successfully
                case "payment_intent.succeeded":
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent != null)
                    {
                        await UpdateOrderPaymentStatus(
                            paymentIntent.Id,
                            "Done",     // PaymentStatus: Processing -> Done
                            "Paid");    // Status: Pending -> Paid
                        
                        _logger.LogInformation(
                            "Payment succeeded for PaymentIntent: {PaymentIntentId}", 
                            paymentIntent.Id);
                    }
                    break;

                // Checkout session completed (important for Stripe Checkout flow)
                case "checkout.session.completed":
                    var session = stripeEvent.Data.Object as Session;
                    if (session != null && !string.IsNullOrEmpty(session.PaymentIntentId))
                    {
                        await UpdateOrderPaymentStatus(
                            session.PaymentIntentId,
                            "Done",     // PaymentStatus: Processing -> Done
                            "Paid");    // Status: Pending -> Paid
                        
                        _logger.LogInformation(
                            "Checkout session completed for PaymentIntent: {PaymentIntentId}", 
                            session.PaymentIntentId);
                    }
                    break;

                // Payment failed
                case "payment_intent.payment_failed":
                    var failedIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (failedIntent != null)
                    {
                        await UpdateOrderPaymentStatus(
                            failedIntent.Id, 
                            "Failed",      // PaymentStatus
                            "Cancelled");  // Status
                        
                        _logger.LogWarning(
                            "Payment failed for PaymentIntent: {PaymentIntentId}", 
                            failedIntent.Id);
                    }
                    break;

                // Payment cancelled
                case "payment_intent.canceled":
                    var canceledIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (canceledIntent != null)
                    {
                        await UpdateOrderPaymentStatus(
                            canceledIntent.Id, 
                            "Cancelled",   // PaymentStatus
                            "Cancelled");  // Status
                        
                        _logger.LogInformation(
                            "Payment cancelled for PaymentIntent: {PaymentIntentId}", 
                            canceledIntent.Id);
                    }
                    break;

                default:
                    _logger.LogInformation(
                        "Unhandled webhook event type: {EventType}", 
                        stripeEvent.Type);
                    break;
            }

            return Ok(new { received = true, eventType = stripeEvent.Type });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Stripe webhook");
            return StatusCode(500, new { error = "Webhook processing failed" });
        }
    }

    /// <summary>
    /// Update order payment status based on payment intent
    /// </summary>
    private async Task UpdateOrderPaymentStatus(
        string paymentIntentId, 
        string paymentStatus, 
        string orderStatus)
    {
        try
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);

            if (order == null)
            {
                _logger.LogWarning(
                    "Order not found for PaymentIntent: {PaymentIntentId}", 
                    paymentIntentId);
                return;
            }

            // Idempotency guard: skip if already at desired statuses
            if (string.Equals(order.PaymentStatus, paymentStatus, StringComparison.OrdinalIgnoreCase)
                && string.Equals(order.Status, orderStatus, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation(
                    "Order {OrderId} already at desired status. PaymentStatus: {PaymentStatus}, Status: {Status}",
                    order.OrderId, order.PaymentStatus, order.Status);
                return;
            }

            _logger.LogInformation(
                "Updating order {OrderId}: PaymentStatus from '{OldPaymentStatus}' to '{NewPaymentStatus}', " +
                "Status from '{OldStatus}' to '{NewStatus}'",
                order.OrderId, 
                order.PaymentStatus, 
                paymentStatus,
                order.Status, 
                orderStatus);

            order.PaymentStatus = paymentStatus;
            order.Status = orderStatus;
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Successfully updated order {OrderId} for PaymentIntent {PaymentIntentId}", 
                order.OrderId, 
                paymentIntentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Error updating order for PaymentIntent: {PaymentIntentId}", 
                paymentIntentId);
            throw;
        }
    }

    /// <summary>
    /// Test endpoint to verify webhook is reachable (for development only)
    /// </summary>
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new 
        { 
            message = "Webhook endpoint is reachable",
            timestamp = DateTime.UtcNow 
        });
    }
}
