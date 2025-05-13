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
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasOne(b => b.Customer)
                .WithOne(l => l.Wallet)
                .HasForeignKey<Wallet>(b => b.CustomerId);

            builder.HasMany(b => b.Deposits)
                .WithOne(l => l.Wallet)
                .HasForeignKey(b => b.WalletId);

            builder.Property(lm => lm.Balance).HasPrecision(18, 2);
        }
    }
}
