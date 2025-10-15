using ECommerce.Web.Models;

namespace ECommerce.Web.Services;

public interface ICartService
{
    Task<CartViewModel?> GetCartAsync();
    Task<bool> AddToCartAsync(int productId, int quantity = 1);
    Task<bool> UpdateQuantityAsync(int cartItemId, int quantity);
    Task<bool> RemoveItemAsync(int cartItemId);
    Task<bool> ClearCartAsync();
}
