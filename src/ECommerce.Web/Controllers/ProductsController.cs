using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;
using ECommerce.Core.Models;

namespace ECommerce.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        // GET: Products
        public async Task<IActionResult> Index(
            string? searchTerm, 
            decimal? minPrice, 
            decimal? maxPrice, 
            string? sortBy, 
            int pageNumber = 1)
        {
            var result = await _productService.GetProductsAsync(searchTerm, minPrice, maxPrice, sortBy, pageNumber);
            
            // Pass filter values to view for form persistence
            ViewBag.SearchTerm = searchTerm;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;
            
            return View(result);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.CreateProductAsync(product, ImageFile);
                    TempData["SuccessMessage"] = "Product created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating product");
                    TempData["ErrorMessage"] = $"Error creating product: {ex.Message}";
                }
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateProductAsync(id, product);
                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating product {Id}", id);
                    TempData["ErrorMessage"] = "Error updating product. Please try again.";
                }
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {Id}", id);
                TempData["ErrorMessage"] = "Error deleting product. Please try again.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}