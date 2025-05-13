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
    public class StickerConfiguration : IEntityTypeConfiguration<Sticker>
    {
        public void Configure(EntityTypeBuilder<Sticker> builder)
        {
            builder.Property(s => s.Name)
                .HasMaxLength(50);

            builder.HasOne(s => s.StickerStyle)
            .WithMany(c => c.Stickers)
            .HasForeignKey(s => s.StickerStyleId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.CreatedBy)
                .WithMany(l => l.CreatedStickers)
                .HasForeignKey(b => b.CreatedById);

            builder.HasOne(b => b.LastModifiedBy)
                .WithMany(l => l.LastModifiedStickers)
                .HasForeignKey(b => b.LastModifiedById);
        }
    }
}
