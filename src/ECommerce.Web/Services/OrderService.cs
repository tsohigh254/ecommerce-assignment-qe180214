using System.Net.Http.Headers;
using System.Text.Json;
using ECommerce.Web.Models;

namespace ECommerce.Web.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrderService> _logger;
    private readonly string _apiBaseUrl;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<OrderService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        // Priority: Environment Variable -> Configuration -> Development Default
        _apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL")
            ?? configuration["ApiSettings:BaseUrl"] 
            ?? "http://api:8080";
        _httpContextAccessor = httpContextAccessor;
    }

    private void AddAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<OrderViewModel?> PlaceOrderAsync(string? paymentIntentId = null)
    {
        try
        {
            AddAuthHeader();
            
            // Create request content with payment intent ID if provided
            StringContent? requestContent = null;
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                var requestData = new { PaymentIntentId = paymentIntentId };
                var json = JsonSerializer.Serialize(requestData);
                requestContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            }
            
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/orders", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var orderDto = JsonSerializer.Deserialize<OrderDto>(content, options);

                if (orderDto != null)
                {
                    return MapToViewModel(orderDto);
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to place order. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error placing order");
        }

        return null;
    }

    public async Task<List<OrderViewModel>> GetOrdersAsync()
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/orders");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var orderDtos = JsonSerializer.Deserialize<List<OrderDto>>(content, options);

                if (orderDtos != null)
                {
                    return orderDtos.Select(MapToViewModel).ToList();
                }
            }
            else
            {
                _logger.LogError($"Failed to get orders. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting orders");
        }

        return new List<OrderViewModel>();
    }

    public async Task<OrderViewModel?> GetOrderByIdAsync(int orderId)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/orders/{orderId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var orderDto = JsonSerializer.Deserialize<OrderDto>(content, options);

                if (orderDto != null)
                {
                    return MapToViewModel(orderDto);
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogWarning($"Order {orderId} not found");
            }
            else
            {
                _logger.LogError($"Failed to get order {orderId}. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting order {orderId}");
        }

        return null;
    }

    private OrderViewModel MapToViewModel(OrderDto dto)
    {
        return new OrderViewModel
        {
            OrderId = dto.OrderId,
            UserId = dto.UserId,
            TotalAmount = dto.TotalAmount,
            Status = dto.Status,
            OrderDate = dto.OrderDate,
            PaymentIntentId = dto.PaymentIntentId,
            PaymentStatus = dto.PaymentStatus,
            Items = dto.Items.Select(i => new OrderItemViewModel
            {
                OrderItemId = i.OrderItemId,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductImageUrl = i.ProductImageUrl,
                Quantity = i.Quantity,
                Price = i.Price,
                Subtotal = i.Subtotal
            }).ToList(),
            TotalItems = dto.TotalItems
        };
    }
}

// DTOs for deserialization
public class OrderDto
{
    public int OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    public int TotalItems { get; set; }
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
