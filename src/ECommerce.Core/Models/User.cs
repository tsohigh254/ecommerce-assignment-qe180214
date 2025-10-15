using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Models
{
    /// <summary>
    /// Application user extending IdentityUser for authentication
    /// </summary>
    public class User : IdentityUser
    {
        // Additional user properties can be added here
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
