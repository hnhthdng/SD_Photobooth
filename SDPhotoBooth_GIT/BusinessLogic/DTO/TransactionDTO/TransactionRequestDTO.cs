using BussinessObject.Enums;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.TransactionDTO
{
    public class TransactionRequestDTO
    {
        public int PaymentId { get; set; }

        public decimal Amount { get; set; }

        public TransactionType Type { get; set; } = TransactionType.Purchase;

        public string? Description { get; set; }

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