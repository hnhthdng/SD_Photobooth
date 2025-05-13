using AutoMapper;
using BusinessLogic.DTO.CoordinateDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class CoordinateService : ICoordinateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CoordinateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> DeleteAllCoordinatesByFrameId(int frameId)
        {
            var coordinates = await _unitOfWork.Coordinate.GetAllAsync(c => c.FrameId == frameId);
            if (coordinates == null || !coordinates.Any())
            {
                return false;
            }
            foreach (var coordinate in coordinates)
            {
                _unitOfWork.Coordinate.Remove(coordinate);
            }
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<CoordinateResponseDTO> CreateCoordinate(CoordinateRequestDTO coordinate)
        {
            var coordinateObj = _mapper.Map<Coordinate>(coordinate);
            await _unitOfWork.Coordinate.AddAsync(coordinateObj);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CoordinateResponseDTO>(coordinateObj);
        }

        public async Task<CoordinateResponseDTO> DeleteCoordinate(int id)
        {
            var coordinateObj = await _unitOfWork.Coordinate.GetFirstOrDefaultAsync(c => c.Id == id);
            if (coordinateObj == null)
            {
                return null;
            }
            _unitOfWork.Coordinate.Remove(coordinateObj);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CoordinateResponseDTO>(coordinateObj);
        }

        public async Task<CoordinateResponseDTO> GetCoordinate(int id)
        {
            var coordinateObj = await _unitOfWork.Coordinate.GetFirstOrDefaultAsync(c => c.Id == id, includeProperties:"Frame");
            if (coordinateObj == null)
            {
                return null;
            }
            return _mapper.Map<CoordinateResponseDTO>(coordinateObj);
        }

        public async Task<IEnumerable<CoordinateResponseDTO>> GetCoordinateByFrameId(int frameId)
        {
            var coordinateObj = await _unitOfWork.Coordinate.GetAllAsync(c => c.FrameId == frameId, includeProperties: "Frame");
            if (coordinateObj == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<CoordinateResponseDTO>>(coordinateObj);
        }

        public async Task<CoordinateResponseDTO> UpdateCoordinate(int id, UpdateCoordinateDTO coordinate)
        {
            var coordinateObj = await _unitOfWork.Coordinate.GetFirstOrDefaultAsync(c => c.Id == id);
            if (coordinateObj == null)
            {
                return null;
            }
            if (coordinate.X.HasValue)
            {
                coordinateObj.X = coordinate.X.Value;
            }
            if (coordinate.Y.HasValue)
            {
                coordinateObj.Y = coordinate.Y.Value;
            }
            if (coordinate.Width.HasValue)
            {
                coordinateObj.Width = coordinate.Width.Value;
            }
            if (coordinate.Height.HasValue)
            {
                coordinateObj.Height = coordinate.Height.Value;
            }
            await _unitOfWork.Coordinate.UpdateAsync(coordinateObj);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CoordinateResponseDTO>(coordinateObj);
        }
    }
}
