using BusinessLogic.DTO.CouponDTO;
using BusinessLogic.DTO.TypeSessionDTO;
using BusinessLogic.DTO.TypeSessionProductDTO;
using BusinessLogic.Service.IService;
using BusinessLogic.Utils;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly ITypeSessionService _typeSessionService;
        private readonly ITypeSessionProductService _typeSessionProductService;
        public CouponController(ICouponService couponService, ITypeSessionService typeSessionService, 
            ITypeSessionProductService typeSessionProductService)
        {
            _couponService = couponService;
            _typeSessionService = typeSessionService;
            _typeSessionProductService = typeSessionProductService;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _couponService.GetCouponCount();
            return Ok(count);
        }

        [HttpGet("by-code/{code}")]
        public async Task<IActionResult> GetCouponByCode(string code)
        {
            var coupon = await _couponService.GetCoupon(code);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        [HttpGet("search/{code}")]
        public async Task<IActionResult> SearchCoupon(string code)
        {
            var coupons = await _couponService.GetCoupons(code);
            if (coupons == null) return NotFound();
            return Ok(coupons);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCouponById(int id)
        {
            var coupon = await _couponService.GetCouponById(id);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        [HttpGet]
        public async Task<IActionResult> GetCoupons([FromQuery] PaginationParams pagination)
        {
            try
            {
                var coupons = await _couponService.GetCoupons(pagination);
                return Ok(coupons);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon([FromBody] CouponRequestDTO couponRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #region Check  discount and discount percent
            var discount = couponRequestDTO.Discount ?? 0;
            var discountPercent = couponRequestDTO.DiscountPercent ?? 0;

            bool hasDiscount = discount > 0;
            bool hasDiscountPercent = discountPercent > 0;

            if (!hasDiscount && !hasDiscountPercent)
            {
                return BadRequest("Phải nhập một trong hai: 'Discount' hoặc 'DiscountPercent' với giá trị lớn hơn 0.");
            }

            if (hasDiscount && hasDiscountPercent)
            {
                return BadRequest("Chỉ được nhập một trong hai: 'Discount' hoặc 'DiscountPercent'.");
            }

            if (discount < 0 || discountPercent < 0)
            {
                return BadRequest("'Discount' và 'DiscountPercent' không được nhỏ hơn 0.");
            }

            #endregion

            var isExist = await _couponService.GetCoupon(couponRequestDTO.Code);
            if (isExist != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Coupon already exists");
            }
            var coupon = await _couponService.CreateCoupon(couponRequestDTO);

            //Create TypeSessionProduct
            var typeSession = await _typeSessionService.GetTypeSessionCodesAsync(new PaginationParams());

            // Filter type session for mobile
            var validTypeSession = new List<TypeSessionResponseDTO>();
            foreach (var ts in typeSession)
            {
                if (ts.ForMobile)
                {
                    try
                    {
                        _couponService.ValidateCoupon(coupon, ts.Price);
                        validTypeSession.Add(ts);
                    }
                    catch { }
                }
            }
            foreach (var ts in validTypeSession)
            {
                var typeSessionProduct = new TypeSessionProductCreateRequestDTO
                {
                    Name = $"{ts.Name}({coupon.Name})", 
                    ProductId = $"product_session_{ProductIdHelper.Normalize(ts.Name)}_{ProductIdHelper.Normalize(coupon.Name)}", 
                    TypeSessionId = ts.Id,
                    CouponId = coupon.Id
                };
                
                await _typeSessionProductService.CreateAsync(typeSessionProduct);
            }

            return Ok(coupon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, [FromBody] CouponRequestDTO couponRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool hasDiscountDTO = couponRequestDTO.Discount.HasValue;
            bool hasDiscountPercentDTO = couponRequestDTO.DiscountPercent.HasValue;

            if (hasDiscountDTO && hasDiscountPercentDTO)
            {
                if (couponRequestDTO.Discount.Value < 0 || couponRequestDTO.DiscountPercent.Value < 0)
                    return BadRequest("Discount and DiscountPercent must not be less than 0.");
                if (couponRequestDTO.Discount.Value > 0 && couponRequestDTO.DiscountPercent.Value > 0)
                    return BadRequest("Only one of 'Discount' or 'DiscountPercent' can be provided.");
                if (couponRequestDTO.Discount.Value == 0 && couponRequestDTO.DiscountPercent.Value == 0)
                    return BadRequest("Either 'Discount' or 'DiscountPercent' must be greater than 0.");

            }
            else if(hasDiscountDTO && !hasDiscountPercentDTO)
            {
                couponRequestDTO.DiscountPercent = 0;
            }
            else if (!hasDiscountDTO && hasDiscountPercentDTO)
            {
                couponRequestDTO.Discount = 0;
            }

            var isExist = await _couponService.GetCouponById(id);
            if (isExist == null)
            {
                return NotFound();
            }
            var coupon = await _couponService.UpdateCoupon(id, couponRequestDTO);
            return Ok(coupon);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var isExist = await _couponService.GetCouponById(id);
            if (isExist == null)
            {
                return NotFound();
            }
            var isDeleted = await _couponService.DeleteCoupon(id);
            var alltypeSessionProducts = await _typeSessionProductService.GetAllAsync(new PaginationParams());
            var typeSessionProducts = alltypeSessionProducts.Where(x => x.CouponId == id).ToList();
            foreach (var item in typeSessionProducts)
            {
                await _typeSessionProductService.DeleteAsync(item.Id);
            }
            return Ok(isDeleted);
        }
    }
}
