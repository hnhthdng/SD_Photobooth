using BusinessLogic.DTO.CouponDTO;
using BusinessLogic.DTO.DepositDTO;
using BusinessLogic.DTO.LevelMembershipDTO;

namespace BusinessLogic.DTO.DepositProductDTO
{
    public class DepositProductResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? AmountAdd { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }
        public string? ProductId { get; set; }
    }
}
