using AutoMapper;
using BusinessLogic.DTO.LocationDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LocationService(IUnitOfWork unitOfWork, IMapper mapper) {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            return await _unitOfWork.Location.CountAsync();
        }
        public async Task<LocationResponseDTO> CreateLocation(LocationRequestDTO locationRequestDTO)
        {

            var location = _mapper.Map<Location>(locationRequestDTO);
            _unitOfWork.Location.Add(location);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<LocationResponseDTO>(location);
        }

        public async Task<int> DeleteLocation(int id)
        {
            var location = await _unitOfWork.Location.GetFirstOrDefaultAsync(filter: e => e.Id == id);
            _unitOfWork.Location.Remove(location);
            await _unitOfWork.SaveAsync();
            return id;
        }

        public async Task<IEnumerable<LocationResponseDTO>> GetAllLocations(PaginationParams? pagination)
        {
            var locations = await _unitOfWork.Location.GetAllAsync(
                pagination: pagination
            );

            return _mapper.Map<IEnumerable<LocationResponseDTO>>(locations);
        }



        public async Task<LocationResponseDTO> GetLocation(int id)
        {
            var location = await _unitOfWork.Location.GetFirstOrDefaultAsync(filter: e => e.Id == id);
            var locationResponseDTO = _mapper.Map<LocationResponseDTO>(location);
            return locationResponseDTO;
        }

        public async Task<IEnumerable<LocationResponseDTO>> GetLocation(string locationName)
        {
            var locations = await _unitOfWork.Location.GetAllAsync(l => l.LocationName.ToLower().Contains(locationName.ToLower()));
            var locationResponseDTO = _mapper.Map<IEnumerable<LocationResponseDTO>>(locations);
            return locationResponseDTO;
        }

        public async Task<LocationResponseDTO> UpdateLocation(int id, LocationRequestDTO locationRequestDTO)
        {
            var location = await _unitOfWork.Location.GetFirstOrDefaultAsync(filter: e => e.Id == id);
            if (locationRequestDTO.LocationName != null)
            {
                location.LocationName = locationRequestDTO.LocationName;
            }
            if (locationRequestDTO.Address != null)
            {
                location.Address = locationRequestDTO.Address;
            }
            _unitOfWork.Location.Update(location);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<LocationResponseDTO>(location);
        }
    }
}
