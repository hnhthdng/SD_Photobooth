using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Enums;

namespace BusinessLogic.DTO.PaymentDTO
{
    public class PaymentResponseDTO
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? DepositId { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentStatus Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative value.")]
        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;
        public long? OrderCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}
