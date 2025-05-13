using AutoMapper;
using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.Service.IService;
using BusinessLogic.Utils;
using BussinessObject.Enums;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> GetCount()
        {
            return await _unitOfWork.Order.CountAsync();
        }

        public async Task<OrderResponseDTO> CreateOrder(OrderRequestDTO orderRequestDTO)
        {
            var order = _mapper.Map<Order>(orderRequestDTO);
            _unitOfWork.Order.Add(order);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> GetOrder(int id, string? userId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(
                includeProperties: "Customer,Payment,Payment.PaymentMethod,Session,Coupon,TypeSession,Session.PhotoHistory,Session.PhotoHistory.Booth,Session.PhotoHistory.Booth.Location",
                filter: e => e.Id == id && (userId == null || e.CustomerId == userId),
                asNoTracking: true
            );

            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> GetOrderByCode(long code, string? userId)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(
                includeProperties: "Customer,Payment,Payment.PaymentMethod,Session,Coupon,TypeSession,Session.PhotoHistory,Session.PhotoHistory.Booth,Session.PhotoHistory.Booth.Location",
                filter: e => e.Code == code && (userId == null || e.CustomerId == userId),
                asNoTracking: true
            );

            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllOrders(PaginationParams? pagination)
        {
            var orders = await _unitOfWork.Order.GetAllAsync(
                includeProperties: "Customer,Payment,Payment.PaymentMethod,Session,Coupon,TypeSession,Session.PhotoHistory.Booth,Session.PhotoHistory.Booth.Location",
                orderBy: o => o.OrderByDescending(o => o.CreatedAt),
                asNoTracking: true,
                pagination: pagination
            );

            var sorted = orders.ToList();

            return _mapper.Map<IEnumerable<OrderResponseDTO>>(sorted);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersForCurrentUser(string userId)
        {
            var orders = await _unitOfWork.Order.GetAllAsync(
                includeProperties: "Customer,Payment,Payment.PaymentMethod,Session,Coupon,TypeSession,Session.PhotoHistory,Session.PhotoHistory.Booth,Session.PhotoHistory.Booth.Location", filter: e => e.CustomerId == userId, asNoTracking: true);

            return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByCusId(string userId)
        {
            var orders = await _unitOfWork.Order.GetAllAsync(
                includeProperties: "Customer,Payment,Payment.PaymentMethod,Session,Coupon,TypeSession,Session.PhotoHistory,Session.PhotoHistory.Booth,Session.PhotoHistory.Booth.Location",
                filter: e => e.CustomerId == userId, asNoTracking: true);

            return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }

        public async Task<long> DeleteOrder(long id)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(filter: e => e.Id == id);
            _unitOfWork.Order.Remove(order);
            await _unitOfWork.SaveAsync();
            return id;
        }

        public async Task<OrderResponseDTO> UpdateOrder(int id, OrderRequestDTO orderRequestDTO)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(
                includeProperties: "Customer,Payment,Payment.PaymentMethod,Session,Coupon,TypeSession,Session.PhotoHistory,Session.PhotoHistory.Booth,Session.PhotoHistory.Booth.Location",
                filter: e => e.Id == id);
            order = _mapper.Map(orderRequestDTO, order);
            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> UpdateOrderStatus(OrderStatusRequestDTO orderStatusRequestDTO)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(filter: e => e.Id == orderStatusRequestDTO.OrderId);
            if (order == null)
            {
                return null;
            }

            order.Status = orderStatusRequestDTO.Status;
            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> UpdateOrderStatusByCode(OrderStatusCodeRequestDTO orderStatusRequestDTO)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(filter: e => e.Code == orderStatusRequestDTO.OrderCode);
            if (order == null)
            {
                return null;
            }
            order.Status = orderStatusRequestDTO.Status;
            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<TotalOrderStaticResponseDTO> StaticOrderCreated(StaticType staticType)
        {
            var totalOrderStatic = new TotalOrderStaticResponseDTO();
            (DateTime start, DateTime startPrev) = TimeRangeHelper.GetTimeRange(staticType);

            var step = TimeRangeHelper.GetStepSize(staticType);

            var groupedCounts = (await _unitOfWork.Order
                .GetAllAsync(s => s.CreatedAt >= startPrev && s.CreatedAt < start.Add(step) && s.Status == OrderStatus.Completed, asNoTracking: true))
                .GroupBy(s => s.CreatedAt >= start ? "current" : "prev")
                .Select(g => new
                {
                    Period = g.Key,
                    Count = g.Count()
                });

            totalOrderStatic.TotalOrder = groupedCounts.FirstOrDefault(x => x.Period == "current")?.Count ?? 0;
            totalOrderStatic.TotalOrderPrev = groupedCounts.FirstOrDefault(x => x.Period == "prev")?.Count ?? 0;

            return totalOrderStatic;
        }

        public async Task<OrderResponseDTO> GetOrderBySessionCode(string sessionCode)
        {
            var order = await _unitOfWork.Order.GetFirstOrDefaultAsync(
                filter: o => o.Session != null && o.Session.Code == sessionCode,
                includeProperties: "Customer,Session");

            if (order == null)
            {
                return null;
            }

            return new OrderResponseDTO
            {
                Id = order.Id,
                Email = order.Customer?.Email,
                Phone = order.Customer?.PhoneNumber,
                SessionCode = order.Session?.Code
            };
        }
    }
}
