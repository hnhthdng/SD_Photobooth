using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class DepositProductConfiguration : IEntityTypeConfiguration<DepositProduct>
    {
        public void Configure(EntityTypeBuilder<DepositProduct> builder)
        {
            builder.Property(lm => lm.Name).HasMaxLength(100);
            builder.Property(lm => lm.Name).IsRequired();
            builder.HasIndex(lm => lm.Name).IsUnique();

            builder.Property(lm => lm.Description).HasMaxLength(500);

            builder.Property(lm => lm.ProductId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(lm => lm.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(lm => lm.AmountAdd)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasMany(lm => lm.Deposits)
                .WithOne(lm => lm.DepositProduct)
                .HasForeignKey(lm => lm.DepositProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
