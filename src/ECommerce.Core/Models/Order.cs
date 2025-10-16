using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Cancelled

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Payment tracking fields
    [StringLength(255)]
    public string? PaymentIntentId { get; set; }

    [StringLength(50)]
    public string PaymentStatus { get; set; } = "Pending"; // Pending, Processing, Paid, Succeeded, Failed, Cancelled

    // Navigation Properties
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    // Computed Properties
    [NotMapped]
    public int TotalItems => OrderItems?.Sum(item => item.Quantity) ?? 0;
}
