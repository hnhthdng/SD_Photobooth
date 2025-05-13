using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.Amount).HasPrecision(18, 2);

            builder.HasOne(x => x.Payment).WithOne(x => x.Transaction).HasForeignKey<Transaction>(x => x.PaymentId);
        }
    }
}