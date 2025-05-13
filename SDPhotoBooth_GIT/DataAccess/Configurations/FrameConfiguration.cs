using BussinessObject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class FrameConfiguration : IEntityTypeConfiguration<Frame>
    {
        public void Configure(EntityTypeBuilder<Frame> builder)
        {
            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(f => f.Coordinates)
                .WithOne(c => c.Frame)
                .HasForeignKey(c => c.FrameId);

            builder.HasOne(f => f.FrameStyle)
                .WithMany(l => l.Frames)
                .HasForeignKey(f => f.FrameStyleId);

            builder.HasOne(b => b.CreatedBy)
                .WithMany(l => l.CreatedFrames)
                .HasForeignKey(b => b.CreatedById);

            builder.HasOne(b => b.LastModifiedBy)
                .WithMany(l => l.LastModifiedFrames)
                .HasForeignKey(b => b.LastModifiedById);
        }
    }
}
