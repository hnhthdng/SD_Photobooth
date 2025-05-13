using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.DepositDTO;
using BussinessObject.Enums;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IDepositService
    {
        Task<IEnumerable<DepositResponseDTO>> GetAllDeposits(PaginationParams? pagination);
        Task<IEnumerable<DepositResponseDTO>> GetDepositsByCusId(string id);
        Task<DepositResponseDTO> CreateDeposit(DepositRequestDTO depositRequestDTO, string userId);
        Task<DepositResponseDTO> UpdateDepositStatusById(int id, DepositStatus depositStatus);
        Task<DepositResponseDTO> DeleteDeposit(int id);
        Task<int> GetCount(); Task<TotalDepositStaticResponseDTO> StaticDepositCreated(StaticType staticType);
    }
}
