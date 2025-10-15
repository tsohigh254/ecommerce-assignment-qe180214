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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWTToken")))
        {
            TempData["ErrorMessage"] = "Please login to view order details";
            return RedirectToAction("Login", "Account");
        }

        var order = await _orderService.GetOrderByIdAsync(id);

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found";
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
}
