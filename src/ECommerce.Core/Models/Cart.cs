using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Models;

public class Cart
{
    [Key]
    public int CartId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    // Computed Properties
    [NotMapped]
    public decimal TotalAmount => CartItems?.Sum(item => item.Subtotal) ?? 0;

    [NotMapped]
    public int TotalItems => CartItems?.Sum(item => item.Quantity) ?? 0;
}
