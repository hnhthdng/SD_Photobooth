using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(o => o.Code)
                .IsUnique();

            builder.Property(o => o.Amount).IsRequired().HasPrecision(18, 2);

            builder.HasOne(o => o.CreatedBy)
                .WithMany(u => u.CreatedOrders)
                .HasForeignKey(o => o.CreatedById);

            builder.HasOne(o => o.LastModifiedBy)
                .WithMany(u => u.LastModifiedOrders)
                .HasForeignKey(o => o.LastModifiedById);

            builder.HasOne(u => u.Session)
              .WithOne(p => p.Order)
              .HasForeignKey<Session>(s => s.OrderId);

            builder.HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(o => o.OrderId);

            builder.HasOne(o => o.TypeSession)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.TypeSessionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Coupon)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CouponId);

            builder.HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId);
        }
    }
}
