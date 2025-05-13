using BussinessObject.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class Payment : BaseEntityNoAudit
    {
        public int? OrderId { get; set; }

        public int? DepositId { get; set; }

        public string? PaymentLink { get; set; }

        public string? OrderCode { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative value.")]
        public decimal Amount { get; set; }

        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod PaymentMethod { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("DepositId")]
        public virtual Deposit Deposit { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}