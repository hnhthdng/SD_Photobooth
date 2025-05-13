using BusinessLogic.DTO.FrameDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IFrameStyleService
    {
        Task<IEnumerable<FrameStyleResponseDTO>> GetFrameStyles(PaginationParams? pagination);
        Task<FrameStyleResponseDTO> GetFrameStyle(int id);
        Task<IEnumerable<FrameStyleResponseDTO>> SearchFrameStyle(string frameStyleName);
        Task<FrameStyleResponseDTO> CreateFrameStyle(FrameStyleRequestDTO frameStyleRequestDTO);
        Task<FrameStyleResponseDTO> UpdateFrameStyle(int id, FrameStyleRequestDTO frameStyleRequestDTO);
        Task<bool> DeleteFrameStyle(int id);
        Task<int> GetCount();
    }
}
