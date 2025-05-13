namespace BusinessLogic.DTO.LevelMembershipDTO
{
    public class LevelMembershipResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Point { get; set; }
        public bool IsActive { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? MaxDiscount { get; set; }
        public decimal? MinOrder { get; set; }
        public int? NextLevelId { get; set; }
    }
}
