using BusinessLogic.DTO.TransactionDTO;

namespace BusinessLogic.DTO.WalletDTO
{
    public class WalletResponseDTO
    {
        public decimal Balance { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public string CustomerId { get; set; }
        public string Id { get; set; }
    }
}
