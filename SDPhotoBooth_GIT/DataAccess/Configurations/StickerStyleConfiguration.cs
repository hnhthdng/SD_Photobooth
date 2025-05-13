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
    public class StickerStyleConfiguration : IEntityTypeConfiguration<StickerStyle>
    {
        public void Configure(EntityTypeBuilder<StickerStyle> builder)
        {
            builder.HasMany(builder => builder.Stickers)
                .WithOne(sticker => sticker.StickerStyle)
                .HasForeignKey(sticker => sticker.StickerStyleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.CreatedBy)
                .WithMany(l => l.CreatedStickerStyles)
                .HasForeignKey(b => b.CreatedById);

            builder.HasOne(b => b.LastModifiedBy)
                .WithMany(l => l.LastModifiedStickerStyles)
                .HasForeignKey(b => b.LastModifiedById);
        }
    }
}
