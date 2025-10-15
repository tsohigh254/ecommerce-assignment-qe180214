using ECommerce.Web.Models;

namespace ECommerce.Web.Services;

public interface IOrderService
{
    Task<OrderViewModel?> PlaceOrderAsync(string? paymentIntentId = null);
    Task<List<OrderViewModel>> GetOrdersAsync();
    Task<OrderViewModel?> GetOrderByIdAsync(int orderId);
}
