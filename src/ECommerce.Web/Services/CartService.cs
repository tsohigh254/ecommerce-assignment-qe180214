using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ECommerce.Web.Models;

namespace ECommerce.Web.Services;

public class CartService : ICartService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CartService> _logger;
    private readonly string _apiBaseUrl;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration, 
        ILogger<CartService> logger,
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

    public async Task<CartViewModel?> GetCartAsync()
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/cart");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var cartDto = JsonSerializer.Deserialize<CartDto>(content, options);
                
                if (cartDto != null)
                {
                    return new CartViewModel
                    {
                        CartId = cartDto.CartId,
                        Items = cartDto.Items.Select(i => new CartItemViewModel
                        {
                            CartItemId = i.CartItemId,
                            ProductId = i.ProductId,
                            ProductName = i.ProductName,
                            ProductPrice = i.ProductPrice,
                            ProductImageUrl = i.ProductImageUrl,
                            Quantity = i.Quantity,
                            Subtotal = i.Subtotal
                        }).ToList(),
                        TotalAmount = cartDto.TotalAmount,
                        TotalItems = cartDto.TotalItems
                    };
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Unauthorized access to cart");
            }
            else
            {
                _logger.LogError($"Failed to get cart. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart");
        }

        return null;
    }

    public async Task<bool> AddToCartAsync(int productId, int quantity = 1)
    {
        try
        {
            AddAuthHeader();
            var addToCartDto = new { productId, quantity };
            var json = JsonSerializer.Serialize(addToCartDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/cart/add", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Added product {productId} to cart");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to add to cart. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding product {productId} to cart");
        }

        return false;
    }

    public async Task<bool> UpdateQuantityAsync(int cartItemId, int quantity)
    {
        try
        {
            AddAuthHeader();
            var updateDto = new { quantity };
            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(
                $"{_apiBaseUrl}/api/cart/item/{cartItemId}", 
                content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Updated cart item {cartItemId} quantity to {quantity}");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to update quantity. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating cart item {cartItemId} quantity");
        }

        return false;
    }

    public async Task<bool> RemoveItemAsync(int cartItemId)
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.DeleteAsync(
                $"{_apiBaseUrl}/api/cart/item/{cartItemId}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Removed cart item {cartItemId}");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to remove item. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error removing cart item {cartItemId}");
        }

        return false;
    }

    public async Task<bool> ClearCartAsync()
    {
        try
        {
            AddAuthHeader();
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/api/cart/clear");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Cart cleared successfully");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to clear cart. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing cart");
        }

        return false;
    }
}

// DTOs for deserialization
public class CartDto
{
    public int CartId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
}

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
