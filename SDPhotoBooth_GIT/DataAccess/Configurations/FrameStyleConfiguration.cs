using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    class FrameStyleConfiguration : IEntityTypeConfiguration<FrameStyle>
    {
        public void Configure(EntityTypeBuilder<FrameStyle> builder)
        {
            builder.Property(f => f.Name)
                .HasMaxLength(50);

            builder.Property(f => f.Description)
                .HasMaxLength(100);

            builder.HasOne(b => b.CreatedBy)
                .WithMany(l => l.CreatedFrameStyles)
                .HasForeignKey(b => b.CreatedById);

            builder.HasOne(b => b.LastModifiedBy)
                .WithMany(l => l.LastModifiedFrameStyles)
                .HasForeignKey(b => b.LastModifiedById);
        }
    }
}
