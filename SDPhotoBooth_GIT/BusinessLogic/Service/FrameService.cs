using AutoMapper;
using BusinessLogic.DTO.CoordinateDTO;
using BusinessLogic.DTO.FrameDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class FrameService : IFrameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoordinateService _coordinateService;
        private readonly IMapper _mapper;

        public FrameService(IUnitOfWork unitOfWork, IMapper mapper, ICoordinateService coordinateService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _coordinateService = coordinateService;
        }
        public async Task<int> GetCount()
        {
            return await _unitOfWork.Frame.CountAsync();
        }
        public async Task<FrameResponseDTO> CreateFrame(FrameRequestDTO frameRequestDTO)
        {
            var frame = _mapper.Map<Frame>(frameRequestDTO);
            await _unitOfWork.Frame.AddAsync(frame);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<FrameResponseDTO>(frame);
        }

        public async Task<bool> DeleteFrame(int id)
        {
            var frame = await _unitOfWork.Frame.GetFirstOrDefaultAsync(f => f.Id == id);
            if (frame == null)
            {
                return false;
            }
            _unitOfWork.Frame.Remove(frame);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<FrameResponseDTO> GetFrame(int id)
        {
            var frames = await _unitOfWork.Frame.GetFirstOrDefaultAsync(f => f.Id == id, includeProperties: "FrameStyle,Coordinates");
            if (frames == null)
            {
                return null;
            }
            frames.Coordinates = frames.Coordinates
                .Where(sticker => !sticker.IsDeleted)
                .ToList();

            return _mapper.Map<FrameResponseDTO>(frames);
        }

        public async Task<IEnumerable<FrameResponseDTO>> GetFrameByFrameStyleId(int frameStyleId)
        {
            var frames = await _unitOfWork.Frame.GetAllAsync(f => f.FrameStyleId == frameStyleId, includeProperties: "FrameStyle,Coordinates");
            foreach (var frame in frames)
            {
                frame.Coordinates = frame.Coordinates
                    .Where(sticker => !sticker.IsDeleted)
                    .ToList();
            }
            return _mapper.Map<IEnumerable<FrameResponseDTO>>(frames);
        }

        public async Task<IEnumerable<FrameResponseDTO>> GetFrames(PaginationParams? pagination)
        {
            var frames = await _unitOfWork.Frame.GetAllAsync(
                includeProperties: "FrameStyle,Coordinates",
                pagination: pagination
            );

            foreach (var frame in frames)
            {
                frame.Coordinates = frame.Coordinates
                    .Where(c => !c.IsDeleted)
                    .ToList();
            }

            return _mapper.Map<IEnumerable<FrameResponseDTO>>(frames);
        }

        public async Task<IEnumerable<FrameResponseDTO>> SearchFrame(string frameName)
        {
            var frames = await _unitOfWork.Frame.GetAllAsync(f => f.Name.ToLower().Contains(frameName.ToLower()), includeProperties: "FrameStyle,Coordinates");
            foreach (var frame in frames)
            {
                frame.Coordinates = frame.Coordinates
                    .Where(sticker => !sticker.IsDeleted)
                    .ToList();
            }
            return _mapper.Map<IEnumerable<FrameResponseDTO>>(frames);
        }

        public async Task<FrameResponseDTO> UpdateFrame(int id, FrameRequestDTO frameRequestDTO)
        {
            var frame = await _unitOfWork.Frame.GetFirstOrDefaultAsync(f => f.Id == id);
            if(!string.IsNullOrEmpty(frameRequestDTO.Name))
            {
                frame.Name = frameRequestDTO.Name;
            }
            if (!string.IsNullOrEmpty(frameRequestDTO.FrameUrl))
            {
                frame.FrameUrl = frameRequestDTO.FrameUrl;
            }
            if (frameRequestDTO.FrameStyleId.HasValue)
            {
                frame.FrameStyleId = frameRequestDTO.FrameStyleId.Value;
            }
            if (frameRequestDTO.SlotCount.HasValue)
            {
                frame.SlotCount = frameRequestDTO.SlotCount.Value;
            }
            if(frameRequestDTO.ForMobile.HasValue)
            {
                frame.ForMobile = frameRequestDTO.ForMobile.Value;
            }
            if (frameRequestDTO.CoordinateDTOs != null && frameRequestDTO.CoordinateDTOs.Any())
            {
                bool result = await _coordinateService.DeleteAllCoordinatesByFrameId(frame.Id);
                if (!result)
                {
                    throw new Exception("Failed to delete coordinates");
                }
                foreach (var coordinate in frameRequestDTO.CoordinateDTOs)
                {
                    var coordinateRequestDTO = new CoordinateRequestDTO
                    {
                        FrameId = frame.Id,
                        X = coordinate.X,
                        Y = coordinate.Y,
                        Width = coordinate.Width,
                        Height = coordinate.Height
                    };
                    await _coordinateService.CreateCoordinate(coordinateRequestDTO);
                }
            }

            await _unitOfWork.SaveAsync();
            return _mapper.Map<FrameResponseDTO>(frame);
        }
    }
}
