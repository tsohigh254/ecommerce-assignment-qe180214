namespace ECommerce.API.Configuration;

/// <summary>
/// Configuration settings for Stripe payment gateway
/// </summary>
public class StripeSettings
{
    /// <summary>
    /// Stripe Secret Key (used on the server-side)
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Stripe Publishable Key (used on the client-side)
    /// </summary>
    public string PublishableKey { get; set; } = string.Empty;

    /// <summary>
    /// Currency code (e.g., "usd", "vnd")
    /// </summary>
    public string Currency { get; set; } = "usd";

    /// <summary>
    /// Whether to use test mode or live mode
    /// </summary>
    public bool TestMode { get; set; } = true;

    /// <summary>
    /// Webhook secret for verifying Stripe webhook signatures
    /// </summary>
    public string WebhookSecret { get; set; } = string.Empty;
}
