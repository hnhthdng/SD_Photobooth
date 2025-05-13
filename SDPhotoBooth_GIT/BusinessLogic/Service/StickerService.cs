using AutoMapper;
using BusinessLogic.DTO.BoothDTO;
using BusinessLogic.DTO.StickerDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class StickerService : IStickerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StickerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            var stickers = await _unitOfWork.Sticker.GetAllAsync();
            return stickers.Count();
        }
        public async Task<StickerResponseDTO> CreateSticker(StickerRequestDTO stickerRequestDTO)
        {
            var sticker = _mapper.Map<Sticker>(stickerRequestDTO);
            await _unitOfWork.Sticker.AddAsync(sticker);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<StickerResponseDTO>(sticker);
        }

        public async Task<StickerResponseDTO> DeleteSticker(int Id)
        {
            var sticker = await _unitOfWork.Sticker.GetFirstOrDefaultAsync(s => s.Id == Id);
            _unitOfWork.Sticker.Remove(sticker);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<StickerResponseDTO>(sticker);

        }

        public async Task<IEnumerable<StickerResponseDTO>> GetAllStickers(PaginationParams? pagination)
        {
            var stickers = await _unitOfWork.Sticker.GetAllAsync(
                includeProperties: "StickerStyle",
                pagination: pagination
            );

            return _mapper.Map<IEnumerable<StickerResponseDTO>>(stickers);
        }


        public async Task<StickerResponseDTO> GetStickerById(int Id)
        {
            var sticker = await _unitOfWork.Sticker.GetFirstOrDefaultAsync(s => s.Id == Id, includeProperties: "StickerStyle");
            return _mapper.Map<StickerResponseDTO>(sticker);
        }

        public async Task<IEnumerable<StickerResponseDTO>> GetStickerByStyleId(int Id)
        {
            var stickers =  await _unitOfWork.Sticker.GetAllAsync(s => s.StickerStyleId == Id, includeProperties:"StickerStyle");
            var stickerResponseDTOs = _mapper.Map<IEnumerable<StickerResponseDTO>>(stickers);
            return stickerResponseDTOs;
        }

        public async Task<StickerResponseDTO> UpdateSticker(int Id, StickerRequestDTO stickerRequestDTO)
        {
            var sticker = await _unitOfWork.Sticker.GetFirstOrDefaultAsync(s => s.Id == Id);
            if(!string.IsNullOrEmpty(stickerRequestDTO.Name))
            {
                sticker.Name = stickerRequestDTO.Name;
            }
            if (!string.IsNullOrEmpty(stickerRequestDTO.StickerUrl))
            {
                sticker.StickerUrl = stickerRequestDTO.StickerUrl;
            }
            if (stickerRequestDTO.StickerStyleId.HasValue)
            {
                sticker.StickerStyleId = stickerRequestDTO.StickerStyleId ?? sticker.StickerStyleId;
            }

            await _unitOfWork.SaveAsync();
            var stickerResponseDTO = _mapper.Map<StickerResponseDTO>(sticker);
            return stickerResponseDTO;
        }
    }
}
