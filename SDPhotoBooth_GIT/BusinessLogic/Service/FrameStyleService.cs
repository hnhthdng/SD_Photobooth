using AutoMapper;
using BusinessLogic.DTO.FrameDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class FrameStyleService : IFrameStyleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FrameStyleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            return await _unitOfWork.FrameStyle.CountAsync();
        }
        public async Task<FrameStyleResponseDTO> CreateFrameStyle(FrameStyleRequestDTO frameStyleRequestDTO)
        {
            var frameStyle = _mapper.Map<FrameStyle>(frameStyleRequestDTO);
            await _unitOfWork.FrameStyle.AddAsync(frameStyle);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<FrameStyleResponseDTO>(frameStyle);
        }

        public async Task<bool> DeleteFrameStyle(int id)
        {
            var frameStyle = await _unitOfWork.FrameStyle.GetFirstOrDefaultAsync(FrameStyle => FrameStyle.Id == id);
            _unitOfWork.FrameStyle.Remove(frameStyle);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<FrameStyleResponseDTO> GetFrameStyle(int id)
        {
            var frameStyle = await _unitOfWork.FrameStyle.GetFirstOrDefaultAsync(FrameStyle => FrameStyle.Id == id);
            return _mapper.Map<FrameStyleResponseDTO>(frameStyle);
        }

        public async Task<IEnumerable<FrameStyleResponseDTO>> GetFrameStyles(PaginationParams? pagination)
        {
            var frameStyles = await _unitOfWork.FrameStyle.GetAllAsync(
                pagination: pagination
            );

            return _mapper.Map<IEnumerable<FrameStyleResponseDTO>>(frameStyles);
        }

        public async Task<IEnumerable<FrameStyleResponseDTO>> SearchFrameStyle(string frameStyleName)
        {
            var frameStyles = await _unitOfWork.FrameStyle.GetAllAsync(FrameStyle => FrameStyle.Name.ToLower().Contains(frameStyleName.ToLower()));
            return _mapper.Map<IEnumerable<FrameStyleResponseDTO>>(frameStyles);
        }

        public async Task<FrameStyleResponseDTO> UpdateFrameStyle(int id, FrameStyleRequestDTO frameStyleRequestDTO)
        {
            var frameStyle = await _unitOfWork.FrameStyle.GetFirstOrDefaultAsync(FrameStyle => FrameStyle.Id == id);
            if(!string.IsNullOrEmpty(frameStyleRequestDTO.Name))
            {
                frameStyle.Name = frameStyleRequestDTO.Name;
            }
            if (!string.IsNullOrEmpty(frameStyleRequestDTO.Description))
            {
                frameStyle.Description = frameStyleRequestDTO.Description;
            }
            if(!string.IsNullOrEmpty(frameStyleRequestDTO.ImageUrl))
            {
                frameStyle.ImageUrl = frameStyleRequestDTO.ImageUrl;
            }
            await _unitOfWork.SaveAsync();
            return _mapper.Map<FrameStyleResponseDTO>(frameStyle);
        }
    }
}
