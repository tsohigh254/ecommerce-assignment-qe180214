using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.Core.Models;
using ECommerce.API.Services;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        private readonly IImageService _imageService;

        public ProductsController(
            ECommerceDbContext context, 
            ILogger<ProductsController> logger,
            IImageService imageService)
        {
            _context = context;
            _logger = logger;
            _imageService = imageService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<ProductPagedResult>> GetProducts(
            [FromQuery] string? searchTerm,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? sortBy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                // Start with all products
                var query = _context.Products.AsQueryable();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var normalizedSearch = searchTerm.Trim().ToLower();
                    query = query.Where(p =>
                        (p.Name != null && p.Name.ToLower().Contains(normalizedSearch)) ||
                        (p.Description != null && p.Description.ToLower().Contains(normalizedSearch)));
                }

                // Apply price filters
                if (minPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= minPrice.Value);
                }

                if (maxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= maxPrice.Value);
                }

                // Apply sorting
                query = sortBy?.ToLower() switch
                {
                    "price_asc" => query.OrderBy(p => p.Price),
                    "price_desc" => query.OrderByDescending(p => p.Price),
                    "name_asc" => query.OrderBy(p => p.Name),
                    "name_desc" => query.OrderByDescending(p => p.Name),
                    "date_desc" => query.OrderByDescending(p => p.CreatedAt),
                    "date_asc" => query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderByDescending(p => p.CreatedAt) // Default: newest first
                };

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var products = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var result = new ProductPagedResult
                {
                    Products = products,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/products
        [HttpPost]
        [Authorize] // Only authenticated users can create products
        public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Handle image upload if provided
                if (dto.ImageFile != null)
                {
                    product.ImageUrl = await _imageService.UploadImageAsync(dto.ImageFile);
                }
                else if (!string.IsNullOrEmpty(dto.ImageUrl))
                {
                    // Allow manual URL input as fallback
                    product.ImageUrl = dto.ImageUrl;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        [Authorize] // Only authenticated users can update products
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest("Product ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                // Update properties
                existingProduct.Name = dto.Name;
                existingProduct.Description = dto.Description;
                existingProduct.Price = dto.Price;

                // Handle image upload if provided
                if (dto.ImageFile != null)
                {
                    existingProduct.ImageUrl = await _imageService.UploadImageAsync(dto.ImageFile);
                }
                else if (!string.IsNullOrEmpty(dto.ImageUrl))
                {
                    // Update URL if provided
                    existingProduct.ImageUrl = dto.ImageUrl;
                }
                // If neither provided, keep existing image

                existingProduct.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product {Id}", id);
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        [Authorize] // Only authenticated users can delete products
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }

    /// <summary>
    /// DTO for creating a product with image upload
    /// </summary>
    public class ProductCreateDto
    {
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [System.ComponentModel.DataAnnotations.Url]
        public string? ImageUrl { get; set; } // Optional URL fallback
        public IFormFile? ImageFile { get; set; } // Image file upload
    }

    /// <summary>
    /// DTO for updating a product with image upload
    /// </summary>
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [System.ComponentModel.DataAnnotations.Url]
        public string? ImageUrl { get; set; } // Optional URL fallback
        public IFormFile? ImageFile { get; set; } // Image file upload
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