using BussinessObject.Models;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.CouponDTO
{
    public class CouponRequestDTO
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Code { get; set; }

        [Range(0, int.MaxValue)]
        public decimal? Discount { get; set; }

        [Range(0, 1, ErrorMessage = "DiscountPercent must be between 0 and 1.")]
        public decimal? DiscountPercent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "MaxUse must be at least 1.")]
        public int? MaxUse { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "UsedAmount must be at least 1.")]
        public int? UsedAmount { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "MaxDiscount must be positive.")]
        public decimal? MaxDiscount { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "MinOrder must be positive.")]
        public decimal? MinOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}
