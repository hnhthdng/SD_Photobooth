using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.TypeSessionProductDTO
{
    public class TypeSessionProductCreateRequestDTO
    {
        public string? Name { get; set; }
        [Required]
        public string ProductId { get; set; }
        public int? LevelMembershipId { get; set; }
        public int TypeSessionId { get; set; }
        public int? CouponId { get; set; }
    }

    public class TypeSessionProductUpdateRequestDTO
    {
        public string? Name { get; set; }
        public string? ProductId { get; set; }
    }
}