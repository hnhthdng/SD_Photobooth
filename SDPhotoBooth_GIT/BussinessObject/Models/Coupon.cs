using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class Coupon : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Discount { get; set; }

        [Required]
        [Range(0, 1)]
        public decimal DiscountPercent { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "MaxUse must be at least 1.")]
        public int? MaxUse { get; set; }

        public int? UsedAmount { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "MaxDiscount must be positive.")]
        public decimal? MaxDiscount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MinOrder must be positive.")]
        public decimal? MinOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<TypeSessionProduct> TypeSessionProducts { get; set; } = new List<TypeSessionProduct>();
    }
}
