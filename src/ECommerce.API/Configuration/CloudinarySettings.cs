using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Configuration;

/// <summary>
/// Configuration settings for Cloudinary image upload service
/// </summary>
public class CloudinarySettings
{
    [Required]
    public string CloudName { get; set; } = string.Empty;
    [Required]
    public string ApiKey { get; set; } = string.Empty;
    [Required]
    public string ApiSecret { get; set; } = string.Empty;
}
