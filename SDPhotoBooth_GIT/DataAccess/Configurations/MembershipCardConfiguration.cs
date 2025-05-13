using BussinessObject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Configurations
{
    public class MembershipCardConfiguration : IEntityTypeConfiguration<MembershipCard>
    {
        public void Configure(EntityTypeBuilder<MembershipCard> builder)
        {
            builder.Property(mc => mc.Description).HasMaxLength(500);

            builder.HasOne(m => m.Customer)
            .WithOne(u => u.MembershipCard)
            .HasForeignKey<MembershipCard>(m => m.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.LevelMemberShip)
            .WithMany(l => l.MembershipCards)
            .HasForeignKey(m => m.LevelMemberShipId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(mc => mc.CreatedBy)
                .WithMany(u => u.CreatedMembershipCards)
                .HasForeignKey(mc => mc.CreatedById);

            builder.HasOne(mc => mc.LastModifiedBy)
                .WithMany(u => u.LastModifiedMembershipCards)
                .HasForeignKey(mc => mc.LastModifiedById);
        }
    }
}
