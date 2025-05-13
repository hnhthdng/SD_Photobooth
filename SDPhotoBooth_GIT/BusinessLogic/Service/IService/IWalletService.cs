using BusinessLogic.DTO.WalletDTO;

namespace BusinessLogic.Service.IService
{
    public interface IWalletService
    {
        Task<WalletResponseDTO> GetWalletByCustomerId(string customerId);
        Task<WalletResponseDTO> CreateWallet(string customerId, WalletRequestDTO walletRequest);
        Task<WalletResponseDTO> UpdateWallet(string customerId, WalletUpdateRequestDTO walletRequest);

        Task<WalletResponseDTO> UpdateBalanceWallet(string customerId, decimal amount, bool isPlus);
    }
}
