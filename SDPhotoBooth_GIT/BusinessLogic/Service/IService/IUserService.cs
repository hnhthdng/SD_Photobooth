using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.UserDTO;
using BussinessObject.Enums;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IUserService
    {
        Task<(IEnumerable<UserResponseDTO> Data, int TotalCount)> GetAllManager(PaginationParams? pagination);
        Task<(IEnumerable<UserResponseDTO> Data, int TotalCount)> GetAllStaff(PaginationParams? pagination);
        Task<(IEnumerable<UserResponseDTO> Data, int TotalCount)> GetAllCustomer(PaginationParams? pagination);
        Task<UserResponseDTO> CreateUser(UserRequestDTO userRequestDTO);
        Task<UserResponseDTO> BanUser(string email);
        Task<UserResponseDTO> UnbanUser(string email);
        Task<UserResponseDTO> UserDetail(string email);
        Task<UserResponseDTO> UserDetailById(string Id);
        Task<bool> ChangeRole(string email, UserType role);
        Task<bool> MoveLocation(string email, int locationId);
        Task<TotalUserStaticResponseDTO> StaticUserCreated(StaticType staticType);
        Task<RevenueStaffStatisticsResponseDTO> StaticRevenue(RevenueFilterDTO revenueFilterDTO);
        Task<RevenueStaffStatisticsResponseDTO> StaticRevenueStaffs(GroupingType staticType);
        Task<int> GetCount();
    }
}