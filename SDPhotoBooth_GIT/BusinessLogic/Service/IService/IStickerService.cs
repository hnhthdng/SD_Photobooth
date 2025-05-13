using BusinessLogic.DTO.StickerDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IStickerService
    {
        Task<IEnumerable<StickerResponseDTO>> GetAllStickers(PaginationParams? pagination);
        Task<StickerResponseDTO> GetStickerById(int Id);
        Task<IEnumerable<StickerResponseDTO>> GetStickerByStyleId(int Id);
        Task<StickerResponseDTO> CreateSticker(StickerRequestDTO stickerRequestDTO);
        Task<StickerResponseDTO> UpdateSticker(int Id, StickerRequestDTO stickerRequestDTO);
        Task<StickerResponseDTO> DeleteSticker(int Id);
        Task<int> GetCount();
    }
}
