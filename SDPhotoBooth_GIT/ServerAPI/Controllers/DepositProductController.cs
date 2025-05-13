using AutoMapper;
using BusinessLogic.DTO.DepositProductDTO;
using BusinessLogic.DTO.ExportProductDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Services.IService;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositProductController : ControllerBase
    {
        private readonly IDepositProductService _depositProductService;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        public DepositProductController(IDepositProductService depositProductService,
            IExportService exportService, IMapper mapper)
        {
            _depositProductService = depositProductService;
            _exportService = exportService;
            _mapper = mapper;
        }

        [HttpGet("count")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _depositProductService.CountAsync();
            return Ok(count);
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Customer")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams? pagination)
        {
            var depositProducts = await _depositProductService.GetAllAsync(pagination);
            return Ok(depositProducts);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var depositProduct = await _depositProductService.GetByIdAsync(id);
            if (depositProduct == null)
            {
                return NotFound();
            }
            return Ok(depositProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _depositProductService.DeleteAsync(id);
            if (!isDeleted)
            {
                return BadRequest("Failed to delete deposit product");
            }
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([FromBody] DepositProductCreateRequestDTO depositProductRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var depositProduct = await _depositProductService.CreateAsync(depositProductRequest);

            if (depositProduct == null)
            {
                return BadRequest("Failed to create deposit product");
            }
            return Ok(depositProduct);
        }

        [HttpGet("export")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ExportCsv()
        {
            var depositProduct = await _depositProductService.GetAllAsync(new PaginationParams());
            if (depositProduct == null)
            {
                return NotFound();
            }

            var products = _mapper.Map<IEnumerable<ExportProductResponseDTO>>(depositProduct);

            var csvBytes = _exportService.ExportProductsToCsv(products);

            return File(csvBytes, "text/csv", "products.csv");
        }
    }
}
