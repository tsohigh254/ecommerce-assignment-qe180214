using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.Services;
using Stripe;

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

            // Handle payment intent succeeded - Update order status
            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    await UpdateOrderPaymentStatus(
                        paymentIntent.Id, 
                        "Succeeded", 
                        "Processing");
                }
            }
            // Handle payment intent payment failed
            else if (stripeEvent.Type == "payment_intent.payment_failed")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    await UpdateOrderPaymentStatus(
                        paymentIntent.Id, 
                        "Failed", 
                        "Cancelled");
                }
            }
            // Handle payment intent cancelled
            else if (stripeEvent.Type == "payment_intent.canceled")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (paymentIntent != null)
                {
                    await UpdateOrderPaymentStatus(
                        paymentIntent.Id, 
                        "Cancelled", 
                        "Cancelled");
                }
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
