using BusinessLogic.DTO.LocationDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface ILocationService
    {
        Task<LocationResponseDTO> CreateLocation(LocationRequestDTO locationRequestDTO);
        Task<LocationResponseDTO> UpdateLocation(int id, LocationRequestDTO locationRequestDTO);
        Task<int> DeleteLocation(int id);
        Task<LocationResponseDTO> GetLocation(int id);
        Task<IEnumerable<LocationResponseDTO>> GetLocation(string locationName);
        Task<IEnumerable<LocationResponseDTO>> GetAllLocations(PaginationParams? pagination);
        Task<int> GetCount();

    }
}
