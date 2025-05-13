using BusinessLogic.DTO.CoordinateDTO;
namespace BusinessLogic.Service.IService

{
    public interface ICoordinateService
    {
        Task<CoordinateResponseDTO> GetCoordinate(int id);
        Task<IEnumerable<CoordinateResponseDTO>> GetCoordinateByFrameId(int frameId);
        Task<CoordinateResponseDTO> CreateCoordinate(CoordinateRequestDTO coordinate);
        Task<CoordinateResponseDTO> UpdateCoordinate(int id, UpdateCoordinateDTO coordinate);
        Task<CoordinateResponseDTO> DeleteCoordinate(int id);
        Task<bool> DeleteAllCoordinatesByFrameId(int frameId);
    }
}
