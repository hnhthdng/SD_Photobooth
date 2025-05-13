using AutoMapper;
using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.DepositDTO;
using BusinessLogic.Service.IService;
using BusinessLogic.Utils;
using BussinessObject.Enums;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class DepositService : IDepositService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepositService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            return await _unitOfWork.Deposit.CountAsync();
        }

        public async Task<DepositResponseDTO> CreateDeposit(DepositRequestDTO depositRequestDTO, string userId)
        {
            var wallet = await _unitOfWork.Wallet.GetFirstOrDefaultAsync(t => t.CustomerId == userId);
            if (wallet == null)
            {
                return null;
            }

            var depositObj = _mapper.Map<Deposit>(depositRequestDTO);

            depositObj.WalletId = wallet.Id;
            await _unitOfWork.Deposit.AddAsync(depositObj);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<DepositResponseDTO>(depositObj);
        }

        public async Task<DepositResponseDTO> GetDeposit(int id)
        {
            var deposit = await _unitOfWork.Deposit.GetFirstOrDefaultAsync(t => t.Id == id);
            if (deposit == null)
            {
                return null;
            }
            return _mapper.Map<DepositResponseDTO>(deposit);
        }

        public async Task<DepositResponseDTO> DeleteDeposit(int id)
        {
            var deposit = await _unitOfWork.Deposit.GetFirstOrDefaultAsync(t => t.Id == id);
            if (deposit == null)
            {
                return null;
            }
            _unitOfWork.Deposit.Remove(deposit);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<DepositResponseDTO>(deposit);
        }

        public async Task<IEnumerable<DepositResponseDTO>> GetAllDeposits(PaginationParams? pagination)
        {
            var deposits = await _unitOfWork.Deposit.GetAllAsync(pagination: pagination);
            return _mapper.Map<IEnumerable<DepositResponseDTO>>(deposits);
        }

        public async Task<DepositResponseDTO> UpdateDepositStatusById(int id, DepositStatus depositStatus)
        {
            var deposit = await _unitOfWork.Deposit.GetFirstOrDefaultAsync(t => t.Id == id);
            if (deposit == null)
            {
                return null;
            }

            deposit.Status = depositStatus;
            _unitOfWork.Deposit.Update(deposit);

            if (depositStatus == DepositStatus.Success)
            {
                var wallet = await _unitOfWork.Wallet.GetFirstOrDefaultAsync(t => t.Id == deposit.WalletId);
                if (wallet != null)
                {
                    wallet.Balance += deposit.Amount;
                    _unitOfWork.Wallet.Update(wallet);
                }
            }
            await _unitOfWork.SaveAsync();
            return _mapper.Map<DepositResponseDTO>(deposit);
        }

        public async Task<IEnumerable<DepositResponseDTO>> GetDepositsByCusId(string id)
        {
            var wallet = await _unitOfWork.Wallet.GetFirstOrDefaultAsync(t => t.CustomerId == id, includeProperties: "Deposits");
            if (wallet == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<DepositResponseDTO>>(wallet.Deposits);
        }


        public async Task<TotalDepositStaticResponseDTO> StaticDepositCreated(StaticType staticType)
        {
            var totalDepositStatic = new TotalDepositStaticResponseDTO();
            (DateTime start, DateTime startPrev) = TimeRangeHelper.GetTimeRange(staticType);

            var step = TimeRangeHelper.GetStepSize(staticType);

            var groupedCounts = (await _unitOfWork.Deposit
                .GetAllAsync(s => s.CreatedAt >= startPrev && s.CreatedAt < start.Add(step) && s.Status == DepositStatus.Success, asNoTracking: true))
                .GroupBy(s => s.CreatedAt >= start ? "current" : "prev")
                .Select(g => new
                {
                    Period = g.Key,
                    Count = g.Count()
                });

            totalDepositStatic.TotalDeposit = groupedCounts.FirstOrDefault(x => x.Period == "current")?.Count ?? 0;
            totalDepositStatic.TotalDepositPrev = groupedCounts.FirstOrDefault(x => x.Period == "prev")?.Count ?? 0;
            return totalDepositStatic;
        }
    }
}
