using ECommerce.Web.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ECommerce.Web.Services;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<PaymentService> logger)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    private void AddAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<PaymentIntentResponse?> CreatePaymentIntentAsync(decimal amount, string currency = "usd")
    {
        try
        {
            AddAuthorizationHeader();

            var requestData = new
            {
                amount = amount,
                currency = currency,
                description = "E-Commerce Order Payment"
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/payment/create-intent", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PaymentIntentResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            else
            {
                _logger.LogError("Failed to create payment intent. Status: {StatusCode}", response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment intent");
            return null;
        }
    }

    public async Task<PaymentStatusResponse?> ConfirmPaymentAsync(string paymentIntentId)
    {
        try
        {
            AddAuthorizationHeader();

            var requestData = new { PaymentIntentId = paymentIntentId };
            var json = JsonSerializer.Serialize(requestData);
            
            _logger.LogInformation("Confirming payment. PaymentIntentId: {PaymentIntentId}, JSON: {Json}", 
                paymentIntentId, json);
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/payment/confirm", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PaymentStatusResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            else
            {
                _logger.LogError("Failed to confirm payment. Status: {StatusCode}", response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming payment");
            return null;
        }
    }

    public async Task<PaymentStatusResponse?> GetPaymentStatusAsync(string paymentIntentId)
    {
        try
        {
            AddAuthorizationHeader();

            var response = await _httpClient.GetAsync($"api/payment/status/{paymentIntentId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PaymentStatusResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            else
            {
                _logger.LogError("Failed to get payment status. Status: {StatusCode}", response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment status");
            return null;
        }
    }

    public async Task<bool> CancelPaymentAsync(string paymentIntentId)
    {
        try
        {
            AddAuthorizationHeader();

            var response = await _httpClient.PostAsync($"api/payment/cancel/{paymentIntentId}", null);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling payment");
            return false;
        }
    }

    public async Task<string?> GetPublishableKeyAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/payment/publishable-key");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PublishableKeyResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result?.PublishableKey;
            }
            else
            {
                _logger.LogError("Failed to get publishable key. Status: {StatusCode}", response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting publishable key");
            return null;
        }
    }
}

// Response models for Payment API
public class PaymentIntentResponse
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
}

public class PaymentStatusResponse
{
    public string PaymentIntentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class PublishableKeyResponse
{
    public string PublishableKey { get; set; } = string.Empty;
}
