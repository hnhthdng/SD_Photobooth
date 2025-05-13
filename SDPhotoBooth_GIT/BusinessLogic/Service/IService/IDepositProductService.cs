using BusinessLogic.DTO.DepositProductDTO;
using BusinessLogic.DTO.TypeSessionProductDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IDepositProductService
    {
        Task<IEnumerable<DepositProductResponseDTO>> GetAllAsync(PaginationParams? pagination);
        Task<DepositProductResponseDTO> GetByIdAsync(int id);
        Task<DepositProductResponseDTO> CreateAsync(DepositProductCreateRequestDTO depositProduct);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
