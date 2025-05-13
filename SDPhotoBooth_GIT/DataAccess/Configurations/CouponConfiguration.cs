using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Discount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(c => c.DiscountPercent)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(c => c.MaxDiscount)
               .HasPrecision(18, 2);

            builder.Property(c => c.MinOrder)
                   .HasPrecision(18, 2);

            builder.Property(c => c.IsActive).HasDefaultValue(true);

            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Coupon)
                .HasForeignKey(o => o.CouponId);

            builder.HasOne(b => b.CreatedBy)
              .WithMany(l => l.CreatedCoupons)
              .HasForeignKey(b => b.CreatedById);

            builder.HasOne(b => b.LastModifiedBy)
                .WithMany(l => l.LastModifiedCoupons)
                .HasForeignKey(b => b.LastModifiedById);
        }
    }
}