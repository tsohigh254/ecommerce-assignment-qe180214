using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Models;

public class CartItem
{
    [Key]
    public int CartItemId { get; set; }

    [Required]
    public int CartId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; } = 1;

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    [ForeignKey(nameof(CartId))]
    public virtual Cart? Cart { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product? Product { get; set; }

    // Computed Properties
    [NotMapped]
    public decimal Subtotal => Product != null ? Product.Price * Quantity : 0;
}
