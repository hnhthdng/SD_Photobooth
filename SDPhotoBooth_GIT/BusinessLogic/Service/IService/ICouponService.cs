using BusinessLogic.DTO.CouponDTO;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.DTO.TypeSessionDTO;
using BusinessLogic.Service;
using DataAccess.Extensions.Pagination;

namespace BusinessLogic.Service.IService
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponResponseDTO>> GetCoupons(PaginationParams? pagination);
        Task<CouponResponseDTO> GetCoupon(string code);
        Task<IEnumerable<CouponResponseDTO>> GetCoupons(string code);
        Task<CouponResponseDTO> GetCouponById(int id);
        Task<CouponResponseDTO> CreateCoupon(CouponRequestDTO couponRequestDTO);
        Task<CouponResponseDTO> UpdateCoupon(int id, CouponRequestDTO couponRequestDTO);
        Task<bool> DeleteCoupon(int id);
        void ValidateCoupon(CouponResponseDTO coupon, decimal price);
        Task<int> GetCouponCount();
        Task<(string? error, OrderRequestDTO order, CouponResponseDTO? coupon)> ApplyCouponIfAny(OrderRequestDTO order, TypeSessionResponseDTO typeSession);
    }
}
