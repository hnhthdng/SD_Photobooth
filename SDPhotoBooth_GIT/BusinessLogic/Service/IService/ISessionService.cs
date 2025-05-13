using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.SessionCodeDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface ISessionService
    {
        Task<SessionResponseDTO> CreateSession(long orderCode);
        Task<IEnumerable<SessionResponseDTO>> SearchSessionByCode(string code);
        Task<IEnumerable<SessionResponseDTO>> GetAllSessions(PaginationParams? pagination);
        Task<SessionResponseDTO> GetSessionById(int Id);
        Task<SessionResponseDTO> GetSessionByCode(string code);
        Task<SessionResponseDTO> UpdateSessionAbleTaken(string code);
        Task<SessionResponseDTO> UseSession(string code, int? boothId);
        Task<UsageChannelStatisticsResponseDTO> StaticUsageChannel(UsageChannelFilterDTO usageChannelFilterDTO);
        Task<int> GetCount();
    }
}