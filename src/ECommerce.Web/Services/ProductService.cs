using ECommerce.Core.Models;

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
        private readonly string _baseUrl;

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
            
            var baseUrl = envVar ?? configValue ?? "http://api:8080";
            
            // Ensure base URL ends with /api
            _baseUrl = baseUrl.TrimEnd('/') + "/api";
            
            _logger.LogInformation("Final API Base URL: {BaseUrl}", _baseUrl);
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
                var response = await _httpClient.GetFromJsonAsync<ProductPagedResult>($"{_baseUrl}/products?{queryString}");
                
                return response ?? new ProductPagedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                return new ProductPagedResult();
            }
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Product>($"{_baseUrl}/products/{id}");
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
                
                var response = await _httpClient.PostAsync($"{_baseUrl}/products", content);
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
                
                var response = await _httpClient.PutAsync($"{_baseUrl}/products/{id}", content);
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
                
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/products/{id}");
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