using BusinessLogic.DTO.TypeSessionDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface ITypeSessionService
    {
        Task<TypeSessionResponseDTO> GetTypeSessionCodeByNameAsync(string name);
        Task<TypeSessionResponseDTO> GetTypeSessionCodeByIdAsync(int id);
        Task<IEnumerable<TypeSessionResponseDTO>> SearchTypeSessionCodeByNameAsync(string name);
        Task<IEnumerable<TypeSessionResponseDTO>> GetTypeSessionCodesAsync(PaginationParams? pagination);
        Task<TypeSessionResponseDTO> CreateTypeSessionCodeAsync(TypeSessionRequestDTO typeSessionCodeDTO);
        Task<bool> UpdateTypeSessionCodeAsync(int id, TypeSessionRequestDTO typeSessionCodeDTO);
        Task<bool> DeleteTypeSessionCodeAsync(int id);
        Task<int> GetCount();

    }
}
