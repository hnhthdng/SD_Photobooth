using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.Property(p => p.Url).IsRequired();

            builder.HasOne(p => p.PhotoHistory)
                .WithMany(ph => ph.Photos)
                .HasForeignKey(p => p.PhotoHistoryId);

            builder.HasOne(p => p.PhotoStyle)
                .WithMany(ph => ph.Photos)
                .HasForeignKey(p => p.PhotoStyleId);
        }
    }
}
