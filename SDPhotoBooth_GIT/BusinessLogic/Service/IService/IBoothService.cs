using BusinessLogic.DTO.BoothDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IBoothService
    {
        Task<IEnumerable<BoothResponseDTO>> GetBooths(PaginationParams? pagination);
        Task<BoothResponseDTO> GetBooth(int id);
        Task<IEnumerable<BoothResponseDTO>> GetBooth(string boothName);
        Task<BoothResponseDTO> CreateBooth(BoothRequestDTO boothRequestDTO);
        Task<BoothResponseDTO> UpdateBooth(int id, BoothRequestDTO boothRequestDTO);
        Task<bool> DeleteBooth(int id);
        Task<IEnumerable<BoothResponseDTO>> GetBoothByLocation(int locationId);
        Task<int> GetBoothCount();
    }
}
