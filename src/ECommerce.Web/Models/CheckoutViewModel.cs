namespace ECommerce.Web.Models;

public class CheckoutViewModel
{
    public CartViewModel Cart { get; set; } = new();
    public string? ClientSecret { get; set; }
    public string? PublishableKey { get; set; }
    public string? PaymentIntentId { get; set; }
    public bool RequiresPayment { get; set; } = true;
}
