using BussinessObject.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Configurations
{
    public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
    {
        public void Configure(EntityTypeBuilder<Deposit> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasOne(c => c.Payment)
                .WithOne(c => c.Deposit)
                .HasForeignKey<Payment>(c => c.DepositId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Wallet)
                .WithMany(c => c.Deposits)
                .HasForeignKey(c => c.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.DepositProduct)
                .WithMany(c => c.Deposits)
                .HasForeignKey(c => c.DepositProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}