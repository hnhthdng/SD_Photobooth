using BusinessLogic.DTO.MembershipCardDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IMembershipCardService
    {
        Task<IEnumerable<MembershipCardResponseDTO>> GetAll(PaginationParams? pagination);
        Task<MembershipCardResponseDTO> GetById(int id);
        Task<MembershipCardResponseDTO> GetByCustomerId(string customerId);
        Task<IEnumerable<MembershipCardResponseDTO>> GetByLevelMembershipId(int levelMembershipId);
        Task<MembershipCardResponseDTO> Create(CreateMembershipCardRequestDTO membershipCardRequestDTO);
        Task<MembershipCardResponseDTO> Update(int id, UpdateMembershipCardRequestDTO membershipCardRequestDTO);
        Task<MembershipCardResponseDTO> UpdatePoint(string cusId, int amount, bool isPlus);
        Task<int> GetCount();
    }
}
