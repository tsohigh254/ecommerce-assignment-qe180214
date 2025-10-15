using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;

namespace ECommerce.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IPaymentService _paymentService;
    private readonly IOrderService _orderService;
    private readonly ILogger<CartController> _logger;

    public CartController(
        ICartService cartService, 
        IPaymentService paymentService,
        IOrderService orderService,
        ILogger<CartController> logger)
    {
        _cartService = cartService;
        _paymentService = paymentService;
        _orderService = orderService;
        _logger = logger;
    }

    // GET: /Cart
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login to view your cart";
            return RedirectToAction("Login", "Account");
        }

        var cart = await _cartService.GetCartAsync();
        
        if (cart == null)
        {
            _logger.LogWarning("Failed to load cart");
            TempData["ErrorMessage"] = "Failed to load cart. Please try again.";
            return View("Error");
        }

        return View(cart);
    }

    // POST: /Cart/Add
    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login to add items to cart";
            return RedirectToAction("Login", "Account");
        }

        var success = await _cartService.AddToCartAsync(productId, quantity);

        if (success)
        {
            TempData["SuccessMessage"] = "Product added to cart successfully!";
            return RedirectToAction("Index");
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to add product to cart. Please try again.";
            return RedirectToAction("Details", "Products", new { id = productId });
        }
    }

    // POST: /Cart/UpdateQuantity
    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            return Json(new { success = false, message = "Please login first" });
        }

        if (quantity < 1)
        {
            return Json(new { success = false, message = "Quantity must be at least 1" });
        }

        var success = await _cartService.UpdateQuantityAsync(cartItemId, quantity);

        if (success)
        {
            return Json(new { success = true, message = "Quantity updated successfully" });
        }
        else
        {
            return Json(new { success = false, message = "Failed to update quantity" });
        }
    }

    // POST: /Cart/Remove
    [HttpPost]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login first";
            return RedirectToAction("Login", "Account");
        }

        var success = await _cartService.RemoveItemAsync(cartItemId);

        if (success)
        {
            TempData["SuccessMessage"] = "Item removed from cart";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to remove item from cart";
        }

        return RedirectToAction("Index");
    }

    // POST: /Cart/Clear
    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login first";
            return RedirectToAction("Login", "Account");
        }

        var success = await _cartService.ClearCartAsync();

        if (success)
        {
            TempData["SuccessMessage"] = "Cart cleared successfully";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to clear cart";
        }

        return RedirectToAction("Index");
    }

    // GET: /Cart/Checkout
    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login to checkout";
            return RedirectToAction("Login", "Account");
        }

        var cart = await _cartService.GetCartAsync();

        if (cart == null || cart.Items.Count == 0)
        {
            TempData["ErrorMessage"] = "Your cart is empty";
            return RedirectToAction("Index", "Products");
        }

        // Create payment intent
        var paymentIntent = await _paymentService.CreatePaymentIntentAsync(cart.TotalAmount);

        if (paymentIntent == null)
        {
            TempData["ErrorMessage"] = "Failed to initialize payment. Please try again.";
            return RedirectToAction("Index");
        }

        var model = new Models.CheckoutViewModel
        {
            Cart = cart,
            ClientSecret = paymentIntent.ClientSecret,
            PublishableKey = paymentIntent.PublishableKey,
            PaymentIntentId = paymentIntent.PaymentIntentId
        };

        return View(model);
    }

    // POST: /Cart/ProcessPayment
    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            return Json(new { success = false, message = "Please login first" });
        }

        if (request == null || string.IsNullOrEmpty(request.PaymentIntentId))
        {
            return Json(new { success = false, message = "Payment Intent ID is required" });
        }

        try
        {
            // Confirm payment with Stripe
            var paymentStatus = await _paymentService.ConfirmPaymentAsync(request.PaymentIntentId);

            if (paymentStatus == null || !paymentStatus.IsSuccessful)
            {
                return Json(new { 
                    success = false, 
                    message = paymentStatus?.ErrorMessage ?? "Payment failed" 
                });
            }

            // Place order with payment intent ID
            var order = await _orderService.PlaceOrderAsync(request.PaymentIntentId);

            if (order == null)
            {
                // Payment succeeded but order creation failed
                _logger.LogError("Payment succeeded but order creation failed. PaymentIntentId: {PaymentIntentId}", request.PaymentIntentId);
                return Json(new { 
                    success = false, 
                    message = "Payment processed but order creation failed. Please contact support." 
                });
            }

            return Json(new { 
                success = true, 
                orderId = order.OrderId,
                message = "Payment successful" 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment");
            return Json(new { success = false, message = "An error occurred processing payment" });
        }
    }

    // GET: /Cart/PaymentSuccess
    [HttpGet]
    public async Task<IActionResult> PaymentSuccess(int orderId)
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login first";
            return RedirectToAction("Login", "Account");
        }

        var order = await _orderService.GetOrderByIdAsync(orderId);

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found";
            return RedirectToAction("Index", "Orders");
        }

        return View(order);
    }

    // GET: /Cart/PaymentFailed
    [HttpGet]
    public IActionResult PaymentFailed(string? message = null)
    {
        ViewBag.ErrorMessage = message ?? "Payment failed. Please try again.";
        return View();
    }

    // GET: /Cart/GetCartItemCount - For navbar badge
    [HttpGet]
    public async Task<IActionResult> GetCartItemCount()
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            return Json(0);
        }

        try
        {
            var cart = await _cartService.GetCartAsync();
            var count = cart?.Items?.Sum(i => i.Quantity) ?? 0;
            return Json(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cart item count");
            return Json(0);
        }
    }
}

// DTO for ProcessPayment request
public class ProcessPaymentRequest
{
    public string PaymentIntentId { get; set; } = string.Empty;
}
