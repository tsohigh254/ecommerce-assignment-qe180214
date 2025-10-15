using ECommerce.Web.Models;

namespace ECommerce.Web.Services;

public interface IPaymentService
{
    Task<PaymentIntentResponse?> CreatePaymentIntentAsync(decimal amount, string currency = "usd");
    Task<PaymentStatusResponse?> ConfirmPaymentAsync(string paymentIntentId);
    Task<PaymentStatusResponse?> GetPaymentStatusAsync(string paymentIntentId);
    Task<bool> CancelPaymentAsync(string paymentIntentId);
    Task<string?> GetPublishableKeyAsync();
}
