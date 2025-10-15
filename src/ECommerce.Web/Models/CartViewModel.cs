namespace ECommerce.Web.Models;

public class CartViewModel
{
    public int CartId { get; set; }
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
}

public class CartItemViewModel
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public string? ProductImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
