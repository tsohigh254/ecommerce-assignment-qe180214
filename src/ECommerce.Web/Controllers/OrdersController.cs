using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Services;

namespace ECommerce.Web.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    // GET: /Orders
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login to view your orders";
            return RedirectToAction("Login", "Account");
        }

        var orders = await _orderService.GetOrdersAsync();
        return View(orders);
    }

    // GET: /Orders/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        // Check if user is logged in
        var token = HttpContext.Session.GetString("JWTToken");
        if (string.IsNullOrEmpty(token))
        {
            TempData["ErrorMessage"] = "Your session has expired. Please login again.";
            return RedirectToAction("Login", "Account");
        }

        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
        {
            // Check if token still exists - if yes, order truly doesn't exist
            // If token is gone, session expired
            var tokenAfter = HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(tokenAfter))
            {
                TempData["ErrorMessage"] = "Your session has expired. Please login again to view your orders.";
                return RedirectToAction("Login", "Account");
            }
            
            TempData["ErrorMessage"] = "Order not found or you don't have permission to view it.";
            return RedirectToAction("Index");
        }

        return View(order);
    }

    // POST: /Orders/Place
    [HttpPost]
    public async Task<IActionResult> Place()
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login to place an order";
            return RedirectToAction("Login", "Account");
        }

        var order = await _orderService.PlaceOrderAsync();

        if (order != null)
        {
            TempData["SuccessMessage"] = $"Order placed successfully! Order ID: {order.OrderId}";
            return RedirectToAction("Details", new { id = order.OrderId });
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to place order. Please try again.";
            return RedirectToAction("Checkout", "Cart");
        }
    }

    // GET: /Orders/GetOrderStatus?id=5 - AJAX endpoint for real-time status check
    [HttpGet]
    public async Task<IActionResult> GetOrderStatus(int id)
    {
        // Check if user is logged in
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            return Json(new { error = "Not authenticated" });
        }

        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
        {
            return Json(new { error = "Order not found" });
        }

        return Json(new 
        { 
            orderId = order.OrderId,
            orderStatus = order.Status,
            paymentStatus = order.PaymentStatus
        });
    }
}
