using BussinessObject.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.LevelMembershipDTO
{
    public class CreateLevelMembershipRequestDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative value.")]
        public int? Point { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public decimal? DiscountPercent { get; set; }

        public decimal? MaxDiscount { get; set; }

        public decimal? MinOrder { get; set; }
        public int? NextLevelId { get; set; } = null;
    }

    public class UpdateLevelMembershipRequestDTO
    {
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative value.")]
        public int? Point { get; set; } 

        public bool? IsActive { get; set; } 

        public decimal? DiscountPercent { get; set; }

        public decimal? MaxDiscount { get; set; }

        public decimal? MinOrder { get; set; }
        public int? NextLevelId { get; set; } = null;
    }
}
