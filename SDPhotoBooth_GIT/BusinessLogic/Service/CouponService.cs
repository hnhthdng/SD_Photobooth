using AutoMapper;
using BusinessLogic.DTO.CouponDTO;
using BusinessLogic.DTO.OrderDTO;
using BusinessLogic.DTO.TypeSessionDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using DataAccess.Repository.IRepository;

namespace BusinessLogic.Service
{
    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CouponService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> GetCouponCount()
        {
            return await _unitOfWork.Coupon.CountAsync();
        }

        public async Task<CouponResponseDTO> CreateCoupon(CouponRequestDTO couponRequestDTO)
        {
            var coupon = _mapper.Map<Coupon>(couponRequestDTO);
            await _unitOfWork.Coupon.AddAsync(coupon);
            await _unitOfWork.SaveAsync();
            var couponResponseDTO = _mapper.Map<CouponResponseDTO>(coupon);
            return couponResponseDTO;
        }

        public async Task<bool> DeleteCoupon(int id)
        {
            var coupon = await _unitOfWork.Coupon.GetFirstOrDefaultAsync(c => c.Id == id);
            _unitOfWork.Coupon.Remove(coupon);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<CouponResponseDTO> GetCoupon(string code)
        {
            var coupon = await _unitOfWork.Coupon.GetFirstOrDefaultAsync(c => c.Code == code);
            var couponResponseDTO = _mapper.Map<CouponResponseDTO>(coupon);
            return couponResponseDTO;
        }

        public async Task<IEnumerable<CouponResponseDTO>> GetCoupons(PaginationParams? pagination)
        {
            var coupons = await _unitOfWork.Coupon.GetAllAsync(
                pagination: pagination
            );

            return _mapper.Map<IEnumerable<CouponResponseDTO>>(coupons);
        }


        public async Task<IEnumerable<CouponResponseDTO>?> GetCoupons(string code)
        {
            var coupons = await _unitOfWork.Coupon.GetAllAsync(c => c.Code.ToLower().Contains(code.ToLower()));
            if (!coupons.Any()) return null;

            return _mapper.Map<IEnumerable<CouponResponseDTO>>(coupons);
        }


        public async Task<CouponResponseDTO> GetCouponById(int id)
        {
            var coupon = await _unitOfWork.Coupon.GetFirstOrDefaultAsync(c => c.Id == id);
            if (coupon == null)
            {
                return null;
            }
            var couponResponseDTO = _mapper.Map<CouponResponseDTO>(coupon);
            return couponResponseDTO;
        }

        public async Task<CouponResponseDTO> UpdateCoupon(int id, CouponRequestDTO couponRequestDTO)
        {
            var existingCoupon = await _unitOfWork.Coupon.GetFirstOrDefaultAsync(c => c.Id == id);

            if (!string.IsNullOrEmpty(couponRequestDTO.Name))
            {
                existingCoupon.Name = couponRequestDTO.Name;
            }
            if (!string.IsNullOrEmpty(couponRequestDTO.Description))
            {
                existingCoupon.Description = couponRequestDTO.Description;
            }
            if (!string.IsNullOrEmpty(couponRequestDTO.Code))
            {
                existingCoupon.Code = couponRequestDTO.Code;
            }
            if (couponRequestDTO.Discount.HasValue)
            {
                existingCoupon.Discount = couponRequestDTO.Discount.Value;
            }
            if (couponRequestDTO.DiscountPercent.HasValue)
            {
                existingCoupon.DiscountPercent = couponRequestDTO.DiscountPercent.Value;
            }
            if (couponRequestDTO.StartDate.HasValue)
            {
                existingCoupon.StartDate = couponRequestDTO.StartDate.Value;
            }
            if (couponRequestDTO.EndDate.HasValue)
            {
                existingCoupon.EndDate = couponRequestDTO.EndDate.Value;
            }
            if (couponRequestDTO.MaxUse.HasValue)
            {
                existingCoupon.MaxUse = couponRequestDTO.MaxUse.Value;
            }
            if (couponRequestDTO.UsedAmount.HasValue)
            {
                existingCoupon.UsedAmount = couponRequestDTO.UsedAmount.Value;
            }
            if (couponRequestDTO.MaxDiscount.HasValue)
            {
                existingCoupon.MaxDiscount = couponRequestDTO.MaxDiscount.Value;
            }
            if (couponRequestDTO.MinOrder.HasValue)
            {
                existingCoupon.MinOrder = couponRequestDTO.MinOrder.Value;
            }
            if (couponRequestDTO.IsActive.HasValue)
            {
                existingCoupon.IsActive = couponRequestDTO.IsActive.Value;
            }

            _unitOfWork.Coupon.Update(existingCoupon);
            await _unitOfWork.SaveAsync();
            var couponResponseDTO = _mapper.Map<CouponResponseDTO>(existingCoupon);
            return couponResponseDTO;
        }

        public void ValidateCoupon(CouponResponseDTO coupon, decimal price)
        {
            if (coupon.StartDate != null && coupon.StartDate > DateTime.Now)
                throw new Exception("The coupon is not yet valid.");

            if (coupon.EndDate != null && coupon.EndDate < DateTime.Now)
                throw new Exception("The coupon has expired.");

            if (coupon.MaxUse != null && coupon.MaxUse <= coupon.UsedAmount)
                throw new Exception("The coupon usage limit has been reached.");

            if (coupon.MinOrder != null && coupon.MinOrder > price)
                throw new Exception($"A minimum order of {coupon.MinOrder:#,##0} is required to apply this coupon.");

            if (!coupon.IsActive)
                throw new Exception("The coupon is currently inactive.");

            if (coupon.IsDeleted)
                throw new Exception("The coupon does not exist.");
        }

        public async Task<(string? error, OrderRequestDTO order, CouponResponseDTO? coupon)> ApplyCouponIfAny(OrderRequestDTO order, TypeSessionResponseDTO typeSession)
        {
            try
            {
                if (string.IsNullOrEmpty(order.CouponCode))
                {
                    order.Amount = typeSession.Price;
                    return (null, order, null);
                }

                var coupon = await GetCoupon(order.CouponCode);
                if (coupon == null)
                {
                    return ("Invalid coupon code.", order, null);
                }

                ValidateCoupon(coupon, typeSession.Price);

                order.CouponId = coupon.Id;
                var discount = Math.Max(0m,
                    coupon.Discount.GetValueOrDefault() +
                    (typeSession.Price * coupon.DiscountPercent.GetValueOrDefault()));

                order.Amount = (coupon.MaxDiscount.HasValue && coupon.MaxDiscount > 0)
                    ? Math.Max(0, typeSession.Price - Math.Min(discount, (decimal)coupon.MaxDiscount.Value))
                    : Math.Max(0, typeSession.Price - discount);

                return (null, order, coupon);
            }
            catch (Exception e)
            {
                return (e.Message, order, null);
            }
        }
    }
}
