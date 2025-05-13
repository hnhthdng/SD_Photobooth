using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.OrderDTO;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrder(OrderRequestDTO locationRequestDTO);
        Task<long> DeleteOrder(long id);
        Task<OrderResponseDTO> UpdateOrder(int id, OrderRequestDTO locationRequestDTO);
        Task<OrderResponseDTO> UpdateOrderStatus(OrderStatusRequestDTO orderStatusRequestDTO);
        Task<OrderResponseDTO> UpdateOrderStatusByCode(OrderStatusCodeRequestDTO orderStatusRequestDTO);
        Task<OrderResponseDTO> GetOrder(int id, string? userId);
        Task<OrderResponseDTO> GetOrderByCode(long code, string? userId);
        Task<IEnumerable<OrderResponseDTO>> GetAllOrders(PaginationParams? pagination);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersForCurrentUser(string userId);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByCusId(string userId);
        Task<TotalOrderStaticResponseDTO> StaticOrderCreated(StaticType staticType);
        Task<OrderResponseDTO> GetOrderBySessionCode(string sessionCode);
        Task<int> GetCount();

    }
}