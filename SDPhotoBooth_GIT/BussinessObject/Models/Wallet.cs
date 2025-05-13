using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class Wallet : BaseEntityNoAudit
    {
        public string CustomerId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a non-negative value.")]
        public decimal Balance { get; set; } = 0;

        public bool IsLocked { get; set; } = false;

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        public virtual ICollection<Deposit> Deposits { get; set; }
    }
}