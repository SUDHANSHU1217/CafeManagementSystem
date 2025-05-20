using CafeManagementSystemBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystemBackend.Data
{
    public class CafeDbContext:DbContext
    {
        public CafeDbContext(DbContextOptions<CafeDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }


        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
          
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cart and CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order and OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // Reservation and Customer
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId);

            // User and Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany()
                .HasForeignKey(c => c.CustomerId);

            // User and Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId);

            // Additional Configurations
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasDefaultValue("Pending");

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.ReservationDate);
        }


    }
}
