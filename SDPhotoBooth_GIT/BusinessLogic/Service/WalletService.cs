using AutoMapper;
using BusinessLogic.DTO.WalletDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WalletResponseDTO> CreateWallet(string customerId, WalletRequestDTO walletRequest)
        {
            var wallet = _mapper.Map<Wallet>(walletRequest);
            wallet.CustomerId = customerId;

            await _unitOfWork.Wallet.AddAsync(wallet);
            await _unitOfWork.SaveAsync();
            var walletResponse = _mapper.Map<WalletResponseDTO>(wallet);
            return walletResponse;
        }

        public async Task<WalletResponseDTO> GetWalletByCustomerId(string customerId)
        {
            var wallet = await _unitOfWork.Wallet.GetFirstOrDefaultAsync(w => w.CustomerId == customerId);

            if (wallet == null)
            {
                return null;
            }
            var walletResponse = _mapper.Map<WalletResponseDTO>(wallet);
            return walletResponse;

        }

        public async Task<WalletResponseDTO> UpdateWallet(string customerId, WalletUpdateRequestDTO walletRequest)
        {
            var wallet = await _unitOfWork.Wallet.GetFirstOrDefaultAsync(w => w.CustomerId == customerId);
            if (wallet == null)
            {
                return null;
            }
            if(walletRequest.Balance.HasValue)
            {
                wallet.Balance = walletRequest.Balance.Value;
            }
            if (walletRequest.IsLocked.HasValue)
            {
                wallet.IsLocked = walletRequest.IsLocked.Value;
            }
            _unitOfWork.Wallet.Update(wallet);
            await _unitOfWork.SaveAsync();
            var walletResponse = _mapper.Map<WalletResponseDTO>(wallet);
            return walletResponse;
        }

        public async Task<WalletResponseDTO> UpdateBalanceWallet(string customerId, decimal amount, bool isPlus)
        {
            var wallet = await _unitOfWork.Wallet.GetFirstOrDefaultAsync(w => w.CustomerId == customerId);
            if (wallet == null)
            {
                return null;
            }

            if (isPlus)
            {
                wallet.Balance += amount;
            }
            else
            {
                if (wallet.Balance < amount)
                {
                    return null;
                }
                wallet.Balance -= amount;
            }

            _unitOfWork.Wallet.Update(wallet);
            await _unitOfWork.SaveAsync();
            var walletResponse = _mapper.Map<WalletResponseDTO>(wallet);
            return walletResponse;
        }
    }
}
