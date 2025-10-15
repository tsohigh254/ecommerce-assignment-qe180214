using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ECommerce.Core.Models;

namespace ECommerce.API.Data
{
    public class ECommerceDbContext : IdentityDbContext<User>
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ImageUrl).HasMaxLength(2083); // Max URL length
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure Cart entity
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.CartId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Carts)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure CartItem entity
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.CartItemId);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.DateAdded).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.Cart)
                    .WithMany(c => c.CartItems)
                    .HasForeignKey(e => e.CartId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.OrderDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure OrderItem entity
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Classic T-Shirt",
                    Description = "Comfortable cotton t-shirt perfect for everyday wear",
                    Price = 29.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Product
                {
                    Id = 2,
                    Name = "Denim Jacket",
                    Description = "Stylish denim jacket for a casual look",
                    Price = 89.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1544966503-7cc5ac882d5f?w=400",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Product
                {
                    Id = 3,
                    Name = "Summer Dress",
                    Description = "Light and breezy summer dress in floral pattern",
                    Price = 59.99m,
                    ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=400",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );
        }
    }
}