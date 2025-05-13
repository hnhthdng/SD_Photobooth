using AutoMapper;
using BusinessLogic.DTO.ExportProductDTO;
using BusinessLogic.DTO.TypeSessionProductDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Services.IService;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeSessionProductController : ControllerBase
    {
        private readonly ITypeSessionProductService _typeSessionProductService;
        private readonly IMembershipCardService _membershipCardService;
        private readonly ICouponService _couponService;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        public TypeSessionProductController(ITypeSessionProductService typeSessionProductService,
             IMembershipCardService membershipCardService,
             ICouponService couponService,
             IExportService exportService, IMapper mapper)
        {
            _typeSessionProductService = typeSessionProductService;
            _membershipCardService = membershipCardService;
            _couponService = couponService;
            _exportService = exportService;
            _mapper = mapper;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _typeSessionProductService.CountAsync();
            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams? pagination)
        {
            var typeSessionProducts = await _typeSessionProductService.GetAllAsync(pagination);
            return Ok(typeSessionProducts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var typeSessionProduct = await _typeSessionProductService.GetByIdAsync(id);
            if (typeSessionProduct == null)
            {
                return NotFound();
            }
            return Ok(typeSessionProduct);
        }

        [HttpGet("GetByTypeSessionId/{typeSessionId}")]
        public async Task<IActionResult> GetByTypeSessionId(int typeSessionId)
        {
            var typeSessionProducts = await _typeSessionProductService.GetByTypeSessionIdAsync(typeSessionId);
            if (typeSessionProducts == null)
            {
                return NotFound();
            }
            return Ok(typeSessionProducts);
        }

        [HttpGet("mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetForMobile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var re = await _membershipCardService.GetByCustomerId(userId);

            var typeSessionProducts = await _typeSessionProductService.GetByLevelMembershipIdAsync(re.LevelMemberShip.Id);


            if(typeSessionProducts == null)
            {
                return NotFound();
            }

            return Ok(typeSessionProducts);

        }

        [HttpGet("coupon")]
        public async Task<IActionResult> GetByCouponAndTypeSessionId([FromQuery] string coupon, [FromQuery] int typeSessionId)
        {
            var isExistCoupon = await _couponService.GetCoupon(coupon);
            if (isExistCoupon == null)
            {
                return NotFound("Coupon not found");
            }
            var typeSessionProducts = await _typeSessionProductService.GetByCouponAndTypeSessionIdAsync(isExistCoupon.Id, typeSessionId);
            if (typeSessionProducts == null)
            {
                return NotFound("Type session product not found");
            }
           
            return Ok(typeSessionProducts);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TypeSessionProductUpdateRequestDTO typeSessionProduct)
        {
            var isExistTypeSessionProduct = await _typeSessionProductService.GetByIdAsync(id);
            if (isExistTypeSessionProduct == null)
            {
                return NotFound();
            }
             
            var updatedTypeSessionProduct = await _typeSessionProductService.UpdateAsync(id, typeSessionProduct);
            return Ok(updatedTypeSessionProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _typeSessionProductService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportCsv()
        {
            var typeSessionProduct = await _typeSessionProductService.GetAllIncludeAsync();
            if (typeSessionProduct == null)
            {
                return NotFound();
            }

            var products = _mapper.Map<IEnumerable<ExportProductResponseDTO>>(typeSessionProduct);

            var csvBytes = _exportService.ExportProductsToCsv(products);

            return File(csvBytes, "text/csv", "products.csv");
        }

    }
}
