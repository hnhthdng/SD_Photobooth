using BussinessObject.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class Transaction : BaseEntityNoAudit
    {
        public int PaymentId { get; set; }

        public string? Description { get; set; }

        public TransactionType Type { get; set; } = TransactionType.Purchase;

        [Required]
        public decimal Amount { get; set; }

        [ForeignKey("PaymentId")]
        public virtual Payment? Payment { get; set; }

        public string? AccountNumber { get; set; }
        public string? Reference { get; set; }
        public string? TransactionDateTime { get; set; }
        public string? Currency { get; set; }
        public string? PaymentLinkId { get; set; }
        public string? Code { get; set; }
        public string? Desc { get; set; }

        public string? CounterAccountBankId { get; set; }
        public string? CounterAccountBankName { get; set; }
        public string? CounterAccountName { get; set; }
        public string? CounterAccountNumber { get; set; }

        public string? VirtualAccountName { get; set; }
        public string? VirtualAccountNumber { get; set; }
    }
}