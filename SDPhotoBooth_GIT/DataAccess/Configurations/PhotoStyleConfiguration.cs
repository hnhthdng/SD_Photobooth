using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PhotoStyleConfiguration : IEntityTypeConfiguration<PhotoStyle>
    {
        public void Configure(EntityTypeBuilder<PhotoStyle> builder)
        {
            builder.Property(ps => ps.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(ps => ps.Description)
                .HasMaxLength(100);

            builder.HasMany(ps => ps.Photos)
                .WithOne(p => p.PhotoStyle)
                .HasForeignKey(p => p.PhotoStyleId);

            builder.HasOne(b => b.CreatedBy)
                .WithMany(l => l.CreatedPhotoStyles)
                .HasForeignKey(b => b.CreatedById);

            builder.HasOne(b => b.LastModifiedBy)
                .WithMany(l => l.LastModifiedPhotoStyles)
                .HasForeignKey(b => b.LastModifiedById);
        }
    }
}
