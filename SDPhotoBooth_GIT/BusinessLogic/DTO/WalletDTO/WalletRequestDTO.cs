using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.WalletDTO
{
    public class WalletRequestDTO
    {
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a non-negative value.")]
        public decimal Balance { get; set; } = 0;

        public bool IsLocked { get; set; } = false;
    }

    public class WalletUpdateRequestDTO
    {
        public decimal? Balance { get; set; }
        public bool? IsLocked { get; set; }
    }
}
