namespace ECommerce.Web.Models;

public class OrderViewModel
{
    public int OrderId { get; set; }
    public int Id => OrderId; // Alias for OrderId
    public string UserId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime CreatedAt { get; set; } // Added for view compatibility
    public List<OrderItemViewModel> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public string? PaymentIntentId { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    
    public string StatusBadgeClass => Status switch
    {
        "Completed" => "badge bg-success",
        "Processing" => "badge bg-info",
        "Pending" => "badge bg-warning",
        "Cancelled" => "badge bg-danger",
        _ => "badge bg-secondary"
    };
    
    public string PaymentStatusBadgeClass => PaymentStatus switch
    {
        "Paid" => "badge bg-success",
        "Succeeded" => "badge bg-success",
        "Processing" => "badge bg-info",
        "Pending" => "badge bg-warning",
        "Failed" => "badge bg-danger",
        "Cancelled" => "badge bg-secondary",
        _ => "badge bg-secondary"
    };
}

public class OrderItemViewModel
{
    public int OrderItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal UnitPrice => Price; // Alias for Price
    public decimal Subtotal { get; set; }
}
