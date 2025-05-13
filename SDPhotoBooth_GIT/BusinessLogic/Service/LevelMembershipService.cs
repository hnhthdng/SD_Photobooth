using AutoMapper;
using BusinessLogic.DTO.LevelMembershipDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class LevelMembershipService : ILevelMembershipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LevelMembershipService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            return await _unitOfWork.LevelMembership.CountAsync();
        }
        public async Task<IEnumerable<LevelMembershipResponseDTO>> GetAllLevelMemberships(PaginationParams? pagination)
        {
            var levelMemberships = await _unitOfWork.LevelMembership.GetAllAsync(
                pagination: pagination
            );

            return _mapper.Map<IEnumerable<LevelMembershipResponseDTO>>(levelMemberships);
        }
        public async Task<LevelMembershipResponseDTO> GetLevelMembershipById(int id)
        {
            var levelMembership = await _unitOfWork.LevelMembership.GetFirstOrDefaultAsync(s => s.Id == id);
            if (levelMembership == null)
            {
                return null;
            }
            var levelMembershipDTO = _mapper.Map<LevelMembershipResponseDTO>(levelMembership);
            return levelMembershipDTO;
        }
        public async Task<LevelMembershipResponseDTO> CreateLevelMembership(CreateLevelMembershipRequestDTO levelMembershipRequestDTO)
        {
            var levelMembership = _mapper.Map<LevelMembership>(levelMembershipRequestDTO);
            await _unitOfWork.LevelMembership.AddAsync(levelMembership);
            await _unitOfWork.SaveAsync();
            var levelMembershipDTO = _mapper.Map<LevelMembershipResponseDTO>(levelMembership);
            return levelMembershipDTO;
        }
        public async Task<LevelMembershipResponseDTO> UpdateLevelMembership(int id, UpdateLevelMembershipRequestDTO levelMembershipRequestDTO)
        {
            var levelMembership = await _unitOfWork.LevelMembership.GetFirstOrDefaultAsync(s => s.Id == id);

            if(!string.IsNullOrEmpty(levelMembershipRequestDTO.Name))
            {
                levelMembership.Name = levelMembershipRequestDTO.Name;
            }
            if (!string.IsNullOrEmpty(levelMembershipRequestDTO.Description))
            {
                levelMembership.Description = levelMembershipRequestDTO.Description;
            }
            if (levelMembershipRequestDTO.Point.HasValue)
            {
                levelMembership.Point = levelMembershipRequestDTO.Point;
            }
            if (levelMembershipRequestDTO.IsActive.HasValue)
            {
                levelMembership.IsActive = levelMembershipRequestDTO.IsActive.Value;
            }
            if (levelMembershipRequestDTO.DiscountPercent.HasValue)
            {
                levelMembership.DiscountPercent = levelMembershipRequestDTO.DiscountPercent;
            }
            if (levelMembershipRequestDTO.MaxDiscount.HasValue)
            {
                levelMembership.MaxDiscount = levelMembershipRequestDTO.MaxDiscount;
            }
            if (levelMembershipRequestDTO.MinOrder.HasValue)
            {
                levelMembership.MinOrder = levelMembershipRequestDTO.MinOrder;
            }
            if (levelMembershipRequestDTO.NextLevelId.HasValue)
            {
                levelMembership.NextLevelId = levelMembershipRequestDTO.NextLevelId;
            }

            await _unitOfWork.LevelMembership.UpdateAsync(levelMembership);
            await _unitOfWork.SaveAsync();
            var levelMembershipDTO = _mapper.Map<LevelMembershipResponseDTO>(levelMembership);
            return levelMembershipDTO;
        }
        public async Task<bool> DeleteLevelMembership(int id)
        {
            var levelMembership = await _unitOfWork.LevelMembership.GetFirstOrDefaultAsync(s => s.Id == id);
            if (levelMembership == null)
            {
                return false;
            }
            _unitOfWork.LevelMembership.Remove(levelMembership);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
