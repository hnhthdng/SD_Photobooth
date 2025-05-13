using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.MembershipCardDTO
{
    public class CreateMembershipCardRequestDTO
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public int LevelMemberShipId { get; set; }
        public string? Description { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; } 
    }

    public class UpdateMembershipCardRequestDTO
    {
        public int? LevelMemberShipId { get; set; }
        public string? Description { get; set; }
        public int? Points { get; set; }
        public bool? IsActive { get; set; }
    }
    public class UpgradeMembershipCardRequestDTO
    {
        public string email { get; set; }
    }
}
