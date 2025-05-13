using BusinessLogic.DTO.LevelMembershipDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface ILevelMembershipService
    {
        Task<IEnumerable<LevelMembershipResponseDTO>> GetAllLevelMemberships(PaginationParams? pagination);
        Task<LevelMembershipResponseDTO> GetLevelMembershipById(int id);
        Task<LevelMembershipResponseDTO> CreateLevelMembership(CreateLevelMembershipRequestDTO levelMembershipRequestDTO);
        Task<LevelMembershipResponseDTO> UpdateLevelMembership(int id, UpdateLevelMembershipRequestDTO levelMembershipRequestDTO);
        Task<bool> DeleteLevelMembership(int id);
        Task<int> GetCount();
    }
}
