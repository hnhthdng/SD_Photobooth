
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
    public class TypeSessionProductConfiguration : IEntityTypeConfiguration<TypeSessionProduct>
    {
        public void Configure(EntityTypeBuilder<TypeSessionProduct> builder)
        {
            builder.Property(lm => lm.Name).HasMaxLength(100);

            builder.HasOne(l => l.TypeSession)
            .WithMany(m => m.TypeSessionProducts)
            .HasForeignKey(m => m.TypeSessionId);

        }
    }
}
