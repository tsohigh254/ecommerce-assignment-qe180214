using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.Configuration
{
    /// <summary>
    /// Configuration settings for JWT authentication
    /// </summary>
    public class JwtSettings
    {
        [Required]
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int ExpirationInMinutes { get; set; } = 60;
    }
}
