using ECommerce.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace ECommerce.Web.Services
{
    public interface IProductService
    {
        Task<ProductPagedResult> GetProductsAsync(string? searchTerm = null, decimal? minPrice = null, 
            decimal? maxPrice = null, string? sortBy = null, int pageNumber = 1, int pageSize = 10);
        Task<Product?> GetProductAsync(int id);
        Task<Product> CreateProductAsync(Product product, IFormFile? imageFile = null);
        Task UpdateProductAsync(int id, Product product, IFormFile? imageFile = null);
        Task DeleteProductAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ProductService> _logger;
        // Loại bỏ _baseUrl, thay thế bằng _apiBasePath

        // Base path cho API (luôn là "/api/"), để ghép nối với đường dẫn tương đối của API
        private const string _apiBasePath = "/api/"; 

        public ProductService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            
            // Priority: Environment Variable -> Configuration -> Development Default
            var envVar = Environment.GetEnvironmentVariable("API_BASE_URL");
            var configValue = configuration["ApiSettings:BaseUrl"];
            
            _logger.LogInformation("API_BASE_URL env var: {EnvVar}", envVar ?? "null");
            _logger.LogInformation("ApiSettings:BaseUrl config: {ConfigValue}", configValue ?? "null");
            
            var baseUrlString = envVar ?? configValue ?? "http://api:8080";
            
            // Clean và sanitize URL string
            baseUrlString = baseUrlString?.Trim() ?? "http://api:8080";
            
            // Log chuỗi sau khi clean để debug
            _logger.LogInformation("Cleaned base URL string: '{BaseUrlString}' (Length: {Length})", 
                baseUrlString, baseUrlString.Length);
            
            // *** CẤU HÌNH QUAN TRỌNG: Thiết lập BaseAddress cho HttpClient ***
            // BaseAddress phải là URI hợp lệ (ví dụ: https://example.com)
            try
            {
                // Đảm bảo URL kết thúc bằng /
                var finalUrl = baseUrlString.TrimEnd('/') + "/";
                _httpClient.BaseAddress = new Uri(finalUrl);
                _logger.LogInformation("HttpClient BaseAddress set to: {BaseAddress}", _httpClient.BaseAddress);
            }
            catch (UriFormatException ex)
            {
                // Log chi tiết để debug
                _logger.LogError(ex, "FATAL: Invalid URI format for API Base URL: '{Url}' | Raw bytes: {Bytes}", 
                    baseUrlString, 
                    string.Join(" ", System.Text.Encoding.UTF8.GetBytes(baseUrlString).Select(b => b.ToString("X2"))));
                throw; 
            }
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<ProductPagedResult> GetProductsAsync(string? searchTerm = null, decimal? minPrice = null, 
            decimal? maxPrice = null, string? sortBy = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var queryParams = new List<string>();
                
                if (!string.IsNullOrEmpty(searchTerm))
                    queryParams.Add($"searchTerm={Uri.EscapeDataString(searchTerm)}");
                
                if (minPrice.HasValue)
                    queryParams.Add($"minPrice={minPrice.Value}");
                
                if (maxPrice.HasValue)
                    queryParams.Add($"maxPrice={maxPrice.Value}");
                
                if (!string.IsNullOrEmpty(sortBy))
                    queryParams.Add($"sortBy={sortBy}");
                
                queryParams.Add($"pageNumber={pageNumber}");
                queryParams.Add($"pageSize={pageSize}");
                
                var queryString = string.Join("&", queryParams);
                
                // *** KHẮC PHỤC DÒNG 77: Sử dụng đường dẫn tương đối (Relative Path)
                var relativePath = $"{_apiBasePath}products?{queryString}";
                
                // Gọi API sử dụng đường dẫn tương đối. HttpClient sẽ tự ghép với BaseAddress
                var response = await _httpClient.GetFromJsonAsync<ProductPagedResult>(relativePath);
                
                return response ?? new ProductPagedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                // Giữ lại dòng này để trả về kết quả rỗng khi gặp lỗi
                return new ProductPagedResult(); 
            }
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            try
            {
                // Sử dụng đường dẫn tương đối
                return await _httpClient.GetFromJsonAsync<Product>($"{_apiBasePath}products/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product {Id}", id);
                return null;
            }
        }

        public async Task<Product> CreateProductAsync(Product product, IFormFile? imageFile = null)
        {
            try
            {
                AddAuthorizationHeader();
                
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(product.Name), "Name");
                content.Add(new StringContent(product.Description), "Description");
                content.Add(new StringContent(product.Price.ToString()), "Price");
                
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    content.Add(new StringContent(product.ImageUrl), "ImageUrl");
                }
                
                if (imageFile != null)
                {
                    var streamContent = new StreamContent(imageFile.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);
                    content.Add(streamContent, "ImageFile", imageFile.FileName);
                }
                
                // Sử dụng đường dẫn tương đối
                var response = await _httpClient.PostAsync($"{_apiBasePath}products", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Product>() ?? product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                throw;
            }
        }

        public async Task UpdateProductAsync(int id, Product product, IFormFile? imageFile = null)
        {
            try
            {
                AddAuthorizationHeader();
                
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(id.ToString()), "Id");
                content.Add(new StringContent(product.Name), "Name");
                content.Add(new StringContent(product.Description), "Description");
                content.Add(new StringContent(product.Price.ToString()), "Price");
                
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    content.Add(new StringContent(product.ImageUrl), "ImageUrl");
                }
                
                if (imageFile != null)
                {
                    var streamContent = new StreamContent(imageFile.OpenReadStream());
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);
                    content.Add(streamContent, "ImageFile", imageFile.FileName);
                }
                
                // Sử dụng đường dẫn tương đối
                var response = await _httpClient.PutAsync($"{_apiBasePath}products/{id}", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {Id}", id);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                
                // Sử dụng đường dẫn tương đối
                var response = await _httpClient.DeleteAsync($"{_apiBasePath}products/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {Id}", id);
                throw;
            }
        }
    }

    /// <summary>
    /// Paged result for products with pagination metadata
    /// </summary>
    public class ProductPagedResult
    {
        public List<Product> Products { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
