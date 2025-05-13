using BussinessObject.Enums;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.PaymentDTO
{
    public class PaymentRequestDTO
    {
        public int? OrderId { get; set; }
        public int? DepositId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentLink { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    }
}