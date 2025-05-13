using BusinessLogic.DTO.StickerStyleDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IStickerStyleService
    {
        Task<IEnumerable<StickerStyleResponseDTO>> GetAllStickerStyles(PaginationParams? pagination);
        Task<StickerStyleResponseDTO> GetStickerStyleById(int Id);
        Task<StickerStyleResponseDTO> CreateStickerStyle(StickerStyleRequestDTO stickerStyleRequestDTO);
        Task<StickerStyleResponseDTO> UpdateStickerStyle(int Id, StickerStyleRequestDTO stickerStyleRequestDTO);
        Task<StickerStyleResponseDTO> DeleteStickerStyle(int Id);
        Task<int> GetCount();
    }
}
