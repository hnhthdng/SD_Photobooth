using AutoMapper;
using BusinessLogic.DTO.BoothDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;
using System.Linq;

namespace BusinessLogic.Service
{
    public class BoothService : IBoothService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BoothService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BoothResponseDTO> CreateBooth(BoothRequestDTO boothRequestDTO)
        {
            var isExist = await _unitOfWork.Booth.GetFirstOrDefaultAsync(b => b.BoothName == boothRequestDTO.BoothName);
            if (isExist != null)
            {
                return null;
            }
            var boothDTO = _mapper.Map<Booth>(boothRequestDTO);
            await _unitOfWork.Booth.AddAsync(boothDTO);
            await _unitOfWork.SaveAsync();
            var booth = await _unitOfWork.Booth.GetFirstOrDefaultAsync(b => b.Id == boothDTO.Id, includeProperties: "Location");
            var boothResponseDTO = _mapper.Map<BoothResponseDTO>(booth);
            return boothResponseDTO;
        }

        public async Task<bool> DeleteBooth(int id)
        {
            var booth = await _unitOfWork.Booth.GetFirstOrDefaultAsync(b => b.Id == id);
            _unitOfWork.Booth.Remove(booth);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<BoothResponseDTO>> GetBooths(PaginationParams? pagination)
        {
            var booths = await _unitOfWork.Booth.GetAllAsync(
                includeProperties: "Location",
                pagination: pagination
            );

            return _mapper.Map<IEnumerable<BoothResponseDTO>>(booths);
        }


        public async Task<BoothResponseDTO> GetBooth(int id)
        {
            var booth = await _unitOfWork.Booth.GetFirstOrDefaultAsync(b => b.Id == id, includeProperties: "Location");
            if (booth == null)
            {
                return null;
            }
            var boothResponseDTO = _mapper.Map<BoothResponseDTO>(booth);
            return boothResponseDTO;
        }

        public async Task<IEnumerable<BoothResponseDTO>> GetBooth(string boothName)
        {
            var booths = await _unitOfWork.Booth.GetAllAsync(b => b.BoothName.ToLower().Contains(boothName.ToLower()), includeProperties: "Location");
            if (booths == null)
            {
                return null;
            }
            var boothResponseDTO = _mapper.Map<IEnumerable<BoothResponseDTO>>(booths);
            return boothResponseDTO;
        }

        public async Task<BoothResponseDTO> UpdateBooth(int id, BoothRequestDTO boothRequestDTO)
        {
            var booth = await _unitOfWork.Booth.GetFirstOrDefaultAsync(b => b.Id == id, includeProperties:"Location");

            if (!string.IsNullOrEmpty(boothRequestDTO.BoothName))
            {
                booth.BoothName = boothRequestDTO.BoothName;
            }
            if (!string.IsNullOrEmpty(boothRequestDTO.Description))
            {
                booth.Description = boothRequestDTO.Description;
            }
            if (boothRequestDTO.LocationId.HasValue)
            {
                var isExistLocation = await _unitOfWork.Location.GetFirstOrDefaultAsync(l => l.Id == boothRequestDTO.LocationId);
                if (isExistLocation == null)
                {
                    return null;
                }
                booth.LocationId = boothRequestDTO.LocationId ?? booth.LocationId;
            }
            if (boothRequestDTO.Status.HasValue)
            {
                booth.Status = boothRequestDTO.Status ?? booth.Status;
            }
            await _unitOfWork.SaveAsync();
            var boothResponseDTO = _mapper.Map<BoothResponseDTO>(booth);
            return boothResponseDTO;
        }

        public async Task<IEnumerable<BoothResponseDTO>> GetBoothByLocation(int locationId)
        {
            var booths = await _unitOfWork.Booth.GetAllAsync(b => b.LocationId == locationId);
            var boothResponseDTOs = _mapper.Map<IEnumerable<BoothResponseDTO>>(booths);
            return boothResponseDTOs;
        }

        public Task<int> GetBoothCount()
        {
            return _unitOfWork.Booth.CountAsync();
        }
    }
}
