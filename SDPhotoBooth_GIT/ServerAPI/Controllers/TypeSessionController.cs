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
    [Authorize(Roles = "Admin")]
    public class TypeSessionController : ControllerBase
    {
        private readonly ITypeSessionService _typeSessionService;
        private readonly ITypeSessionProductService _typeSessionProductService;
        private readonly ILevelMembershipService _levelMembershipService;
        private readonly ICouponService _couponService;
        public TypeSessionController(ITypeSessionService typeSessionService, ITypeSessionProductService typeSessionProductService,
            ILevelMembershipService levelMembershipService, ICouponService couponService)
        {
            _typeSessionService = typeSessionService;
            _typeSessionProductService = typeSessionProductService;
            _levelMembershipService = levelMembershipService;
            _couponService = couponService;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _typeSessionService.GetCount();
            return Ok(count);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] PaginationParams pagination)
        {
            try
            {
                var typeSessions = await _typeSessionService.GetTypeSessionCodesAsync(pagination);
                return Ok(typeSessions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var typeSession = await _typeSessionService.GetTypeSessionCodeByIdAsync(id);
            if (typeSession == null)
            {
                return NotFound();
            }
            return Ok(typeSession);
        }

        [HttpGet("by-name/{typeSessionName}")]
        public async Task<IActionResult> Get(string typeSessionName)
        {
            var typeSession = await _typeSessionService.SearchTypeSessionCodeByNameAsync(typeSessionName);
            if (typeSession == null)
            {
                return NotFound();
            }
            return Ok(typeSession);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TypeSessionRequestDTO typeSessionRequestDTO)
        {
            if (typeSessionRequestDTO == null)
            {
                return BadRequest("Invalid data provided.");
            }
            var isNameExist = await _typeSessionService.GetTypeSessionCodeByNameAsync(typeSessionRequestDTO.Name);
            if (isNameExist != null)
            {
                return BadRequest("TypeSession name already exists.");
            }
            var typeSession = await _typeSessionService.CreateTypeSessionCodeAsync(typeSessionRequestDTO);

            if(typeSession.ForMobile)
            {
                //Create TypeSessionProduct
                var levelMemberships = await _levelMembershipService.GetAllLevelMemberships(new PaginationParams());
                var coupons = await _couponService.GetCoupons(new PaginationParams());

                var validCoupons = new List<CouponResponseDTO>();
                foreach (var c in coupons)
                {
                    try
                    {
                        _couponService.ValidateCoupon(c, typeSession.Price);
                        validCoupons.Add(c);
                    }
                    catch { }
                }


                foreach (var level in levelMemberships)
                {
                    var typeSessionProduct = new TypeSessionProductCreateRequestDTO
                    {
                        Name = $"{typeSession.Name}({level.Name})", 
                        ProductId = $"product_session_{ProductIdHelper.Normalize(typeSession.Name)}_{ProductIdHelper.Normalize(level.Name)}", 
                        TypeSessionId = typeSession.Id,
                        LevelMembershipId = level.Id
                    };

                    await _typeSessionProductService.CreateAsync(typeSessionProduct);
                }

                foreach (var coupon in validCoupons)
                {
                    var typeSessionProduct = new TypeSessionProductCreateRequestDTO
                    {
                        Name = $"{typeSession.Name}({coupon.Name})", 
                        ProductId = $"product_session_{ProductIdHelper.Normalize(typeSession.Name)}_{ProductIdHelper.Normalize(coupon.Name)}", 
                        TypeSessionId = typeSession.Id,
                        CouponId = coupon.Id
                    };
                    await _typeSessionProductService.CreateAsync(typeSessionProduct);
                }
            } 

            return Ok(typeSession);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TypeSessionRequestDTO typeSessionRequestDTO)
        {
            if (typeSessionRequestDTO == null)
            {
                return BadRequest("Invalid data provided.");
            }
            var isNameExist = await _typeSessionService.GetTypeSessionCodeByIdAsync(id);
            if (isNameExist == null)
            {
                return BadRequest("TypeSession not found.");
            }

            var typeSession = await _typeSessionService.UpdateTypeSessionCodeAsync(id, typeSessionRequestDTO);
            return Ok(typeSession);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _typeSessionService.DeleteTypeSessionCodeAsync(id);
            if (!result)
            {
                return NotFound();
            }
            var typeSessionProducts = await _typeSessionProductService.GetByTypeSessionIdAsync(id);
            foreach (var typeSessionProduct in typeSessionProducts)
            {
                await _typeSessionProductService.DeleteAsync(typeSessionProduct.Id);
            }
            return Ok();
        }
    }
}
