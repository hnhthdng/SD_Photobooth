using BusinessLogic.DTO.FrameDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IFrameService
    {
        Task<IEnumerable<FrameResponseDTO>> GetFrames(PaginationParams? pagination);
        Task<FrameResponseDTO> GetFrame(int id);
        Task<IEnumerable<FrameResponseDTO>> GetFrameByFrameStyleId(int frameStyleId);
        Task<IEnumerable<FrameResponseDTO>> SearchFrame(string frameName);
        Task<FrameResponseDTO> CreateFrame(FrameRequestDTO frameRequestDTO);
        Task<FrameResponseDTO> UpdateFrame(int id, FrameRequestDTO frameRequestDTO);
        Task<bool> DeleteFrame(int id);
        Task<int> GetCount();
    }
}
