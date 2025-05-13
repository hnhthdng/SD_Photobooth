using BussinessObject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace DataAccess.Configurations
{
    public class LevelMembershipConfiguration : IEntityTypeConfiguration<LevelMembership>
    {
        public void Configure(EntityTypeBuilder<LevelMembership> builder)
        {
            builder.Property(lm => lm.Name).IsRequired().HasMaxLength(100);

            builder.Property(lm => lm.Description).HasMaxLength(500);

            builder.Property(lm => lm.MaxDiscount).HasPrecision(18, 2);

            builder.Property(lm => lm.MinOrder).IsRequired().HasPrecision(18, 2);

            builder.Property(lm => lm.DiscountPercent).HasPrecision(18, 2);

            builder.HasMany(l => l.MembershipCards)
            .WithOne(m => m.LevelMemberShip)
            .HasForeignKey(m => m.LevelMemberShipId);

            builder.HasOne(b => b.CreatedBy)
               .WithMany(l => l.CreatedLevelMemberships)
               .HasForeignKey(b => b.CreatedById);

            builder.HasOne(b => b.LastModifiedBy)
                .WithMany(l => l.LastModifiedLevelMemberships)
                .HasForeignKey(b => b.LastModifiedById);

            builder.HasMany(b => b.TypeSessionProducts)
                .WithOne(l => l.LevelMembership)
                .HasForeignKey(b => b.LevelMembershipId);

            builder.HasOne(lm => lm.NextLevel)
                    .WithOne() 
                    .HasForeignKey<LevelMembership>(lm => lm.NextLevelId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
