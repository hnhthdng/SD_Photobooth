using BussinessObject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Configurations
{
    public class PhotoHistoryConfiguration : IEntityTypeConfiguration<PhotoHistory>
    {
        public void Configure(EntityTypeBuilder<PhotoHistory> builder)
        {
            builder.HasOne(ph => ph.Booth)
                .WithMany(o => o.PhotoHistories)
                .HasForeignKey(o => o.BoothId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(ph => ph.Photos)
                .WithOne(p => p.PhotoHistory)
                .HasForeignKey(p => p.PhotoHistoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ph => ph.Customer)
                .WithMany(u => u.PhotoHistories)
                .HasForeignKey(ph => ph.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(o => o.Session)
                .WithOne(o => o.PhotoHistory)
                .HasForeignKey<PhotoHistory>(o => o.SessionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
