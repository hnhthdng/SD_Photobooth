using BussinessObject.Enums;

namespace BusinessLogic.DTO.DepositDTO
{
    public class DepositRequestDTO
    {
        public decimal? Amount { get; set; }
        public int DepositProductId { get; set; }
        public int PaymentMethodId { get; set; }
        public DepositStatus Status { get; set; } = DepositStatus.Pending;
        public string? Description { get; set; } = null;
    }
}
