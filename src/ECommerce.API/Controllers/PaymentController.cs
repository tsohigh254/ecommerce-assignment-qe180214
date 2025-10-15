using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Services;
using ECommerce.Core.Models;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Create a payment intent for checkout
    /// </summary>
    /// <param name="request">Payment intent creation request</param>
    /// <returns>Payment intent with client secret</returns>
    [HttpPost("create-intent")]
    public async Task<ActionResult<PaymentIntentResponseDto>> CreatePaymentIntent([FromBody] CreatePaymentIntentDto request)
    {
        try
        {
            if (request.Amount <= 0)
            {
                return BadRequest(new { message = "Amount must be greater than zero" });
            }

            var result = await _paymentService.CreatePaymentIntentAsync(request);
            
            _logger.LogInformation("Payment intent created: {PaymentIntentId} for amount {Amount}", 
                result.PaymentIntentId, request.Amount);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment intent");
            return StatusCode(500, new { message = "An error occurred while creating payment intent", error = ex.Message });
        }
    }

    /// <summary>
    /// Confirm a payment intent
    /// </summary>
    /// <param name="request">Payment confirmation request</param>
    /// <returns>Payment status</returns>
    [HttpPost("confirm")]
    public async Task<ActionResult<PaymentStatusDto>> ConfirmPayment([FromBody] PaymentConfirmDto request)
    {
        try
        {
            _logger.LogInformation("Received confirm payment request. PaymentIntentId: {PaymentIntentId}", 
                request?.PaymentIntentId ?? "NULL");
            
            if (request == null)
            {
                return BadRequest(new { message = "Request body is null" });
            }
            
            if (string.IsNullOrEmpty(request.PaymentIntentId))
            {
                return BadRequest(new { message = "Payment intent ID is required" });
            }

            var result = await _paymentService.ConfirmPaymentAsync(request.PaymentIntentId);
            
            _logger.LogInformation("Payment confirmed: {PaymentIntentId} - Status: {Status}", 
                result.PaymentIntentId, result.Status);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming payment");
            return StatusCode(500, new { message = "An error occurred while confirming payment", error = ex.Message });
        }
    }

    /// <summary>
    /// Get payment status
    /// </summary>
    /// <param name="paymentIntentId">Stripe payment intent ID</param>
    /// <returns>Payment status</returns>
    [HttpGet("status/{paymentIntentId}")]
    public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatus(string paymentIntentId)
    {
        try
        {
            if (string.IsNullOrEmpty(paymentIntentId))
            {
                return BadRequest(new { message = "Payment intent ID is required" });
            }

            var result = await _paymentService.GetPaymentStatusAsync(paymentIntentId);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment status");
            return StatusCode(500, new { message = "An error occurred while getting payment status", error = ex.Message });
        }
    }

    /// <summary>
    /// Cancel a payment intent
    /// </summary>
    /// <param name="paymentIntentId">Stripe payment intent ID</param>
    /// <returns>Success status</returns>
    [HttpPost("cancel/{paymentIntentId}")]
    public async Task<ActionResult> CancelPayment(string paymentIntentId)
    {
        try
        {
            if (string.IsNullOrEmpty(paymentIntentId))
            {
                return BadRequest(new { message = "Payment intent ID is required" });
            }

            var result = await _paymentService.CancelPaymentAsync(paymentIntentId);
            
            if (result)
            {
                _logger.LogInformation("Payment cancelled: {PaymentIntentId}", paymentIntentId);
                return Ok(new { message = "Payment cancelled successfully" });
            }
            else
            {
                return BadRequest(new { message = "Failed to cancel payment" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling payment");
            return StatusCode(500, new { message = "An error occurred while cancelling payment", error = ex.Message });
        }
    }

    /// <summary>
    /// Get Stripe publishable key for client-side integration
    /// </summary>
    /// <returns>Publishable key</returns>
    [HttpGet("publishable-key")]
    [AllowAnonymous]
    public ActionResult<object> GetPublishableKey()
    {
        try
        {
            var publishableKey = _paymentService.GetPublishableKey();
            return Ok(new { publishableKey });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting publishable key");
            return StatusCode(500, new { message = "An error occurred while getting publishable key" });
        }
    }
}
