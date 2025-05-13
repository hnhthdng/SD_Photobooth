using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TypeSessionConfiguration : IEntityTypeConfiguration<TypeSession>
    {
        public void Configure(EntityTypeBuilder<TypeSession> builder)
        {
            builder.Property(ts => ts.Name).IsRequired().HasMaxLength(100);

            builder.Property(ts => ts.Description).HasMaxLength(500);

            builder.Property(lm => lm.Price).HasPrecision(18, 2);

            builder.Property(lm => lm.AbleTakenNumber).IsRequired();

            builder.HasMany(ts => ts.Orders)
                .WithOne(o => o.TypeSession)
                .HasForeignKey(o => o.TypeSessionId);

            builder.HasOne(b => b.CreatedBy)
              .WithMany(l => l.CreatedTypeSessions)
              .HasForeignKey(b => b.CreatedById);

            builder.HasOne(b => b.LastModifiedBy)
                .WithMany(l => l.LastModifiedTypeSessions)
                .HasForeignKey(b => b.LastModifiedById);
        }
    }
}
