using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class DepositProduct : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal AmountAdd { get; set; }

        public IEnumerable<Deposit>? Deposits { get; set; }
    }
}
