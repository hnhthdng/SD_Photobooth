using AutoMapper;
using BusinessLogic.DTO.StickerStyleDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class StickerStyleService : IStickerStyleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StickerStyleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            var stickerStyles = await _unitOfWork.StickerStyle.GetAllAsync();
            return stickerStyles.Count();
        }
        public async Task<StickerStyleResponseDTO> CreateStickerStyle(StickerStyleRequestDTO stickerStyleRequestDTO)
        {
            var stickerStyle = _mapper.Map<StickerStyle>(stickerStyleRequestDTO);
            await _unitOfWork.StickerStyle.AddAsync(stickerStyle);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<StickerStyleResponseDTO>(stickerStyle);
        }

        public async Task<StickerStyleResponseDTO> DeleteStickerStyle(int Id)
        {
            var stickerStyle = await _unitOfWork.StickerStyle.GetFirstOrDefaultAsync(s => s.Id == Id);
            _unitOfWork.StickerStyle.Remove(stickerStyle);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<StickerStyleResponseDTO>(stickerStyle);
        }

        public async Task<IEnumerable<StickerStyleResponseDTO>> GetAllStickerStyles(PaginationParams? pagination)
        {
            var stickerStyles = await _unitOfWork.StickerStyle.GetAllAsync(
                includeProperties: "Stickers",
                pagination: pagination
            );

            foreach (var style in stickerStyles)
            {
                style.Stickers = style.Stickers
                    .Where(sticker => !sticker.IsDeleted)
                    .ToList();
            }

            return _mapper.Map<IEnumerable<StickerStyleResponseDTO>>(stickerStyles);
        }


        public async Task<StickerStyleResponseDTO> GetStickerStyleById(int id)
        {
            var stickerStyle = await _unitOfWork.StickerStyle
                .GetFirstOrDefaultAsync(s => s.Id == id, includeProperties: "Stickers");

            if (stickerStyle == null) return null;

            stickerStyle.Stickers = stickerStyle.Stickers
                .Where(sticker => !sticker.IsDeleted)
                .ToList();

            return _mapper.Map<StickerStyleResponseDTO>(stickerStyle);
        }

        public async Task<StickerStyleResponseDTO> UpdateStickerStyle(int Id, StickerStyleRequestDTO stickerStyleRequestDTO)
        {
            var stickerStyle = await _unitOfWork.StickerStyle.GetFirstOrDefaultAsync(s => s.Id == Id);
            if(!string.IsNullOrEmpty(stickerStyleRequestDTO.StickerStyleName))
            {
                stickerStyle.StickerStyleName = stickerStyleRequestDTO.StickerStyleName;
            }
            if (!string.IsNullOrEmpty(stickerStyleRequestDTO.Description))
            {
                stickerStyle.Description = stickerStyleRequestDTO.Description;
            }
            await _unitOfWork.SaveAsync();
            return _mapper.Map<StickerStyleResponseDTO>(stickerStyle);
        }
    }
}
