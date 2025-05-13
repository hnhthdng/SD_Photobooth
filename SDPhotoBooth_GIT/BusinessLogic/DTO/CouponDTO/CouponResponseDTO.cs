namespace BusinessLogic.DTO.CouponDTO
{
    public class CouponResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPercent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MaxUse { get; set; }
        public decimal? MaxDiscount { get; set; }
        public decimal? MinOrder { get; set; }
        public bool IsActive { get; set; }
        public int? UsedAmount { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }
    }
}
