using BussinessObject.Enums;

namespace BusinessLogic.DTO.MembershipCardDTO
{
    public class MembershipCardResponseDTO
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }

        public CustomerOnMembershipCard Customer { get; set; }
        public LevelMembershipOnMembershipCard LevelMemberShip { get; set; }
    }

    public class CustomerOnMembershipCard
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public UserGender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class LevelMembershipOnMembershipCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Point { get; set; }
        public int? NextLevelId { get; set; }
    }
}
