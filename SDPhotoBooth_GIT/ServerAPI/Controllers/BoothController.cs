using BusinessLogic.DTO.BoothDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Manager")]
    public class BoothController : ControllerBase
    {
        private readonly IBoothService _boothService;
        public BoothController(IBoothService boothService)
        {
            _boothService = boothService;
        }

        //get ra booth da tao cuar th dang dapj 


        [HttpGet]
        public async Task<IActionResult> GetBooths([FromQuery] PaginationParams pagination)
        {
            try
            {
                var booths = await _boothService.GetBooths(pagination);
                return Ok(booths);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BoothResponseDTO>> GetBooth(int id)
        {
            var booth = await _boothService.GetBooth(id);
            if (booth == null)
            {
                return NotFound();
            }
            return Ok(booth);
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<BoothResponseDTO>> GetBoothByName(string name)
        {
            var booth = await _boothService.GetBooth(name);
            if (booth == null)
            {
                return NotFound();
            }
            return Ok(booth);
        }

        [HttpGet("by-location/{locationId}")]
        public async Task<ActionResult<IEnumerable<BoothResponseDTO>>> GetBoothByLocation(int locationId)
        {
            var booths = await _boothService.GetBoothByLocation(locationId);
            return Ok(booths);
        }

        [HttpPost]
        public async Task<ActionResult<BoothResponseDTO>> CreateBooth(BoothRequestDTO boothRequestDTO)
        {
            var booth = await _boothService.CreateBooth(boothRequestDTO);
            if (booth == null)
            {
                return BadRequest("Booth already exists");
            }
            return Ok(booth);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<BoothResponseDTO>> UpdateBooth(int id, BoothRequestDTO boothRequestDTO)
        {
            var isExist = await _boothService.GetBooth(id);
            if (isExist == null)
            {
                return NotFound();
            }
            var booth = await _boothService.UpdateBooth(id, boothRequestDTO);
            if (booth == null)
            {
                return BadRequest("Failed to update booth");
            }
            return Ok(booth);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BoothResponseDTO>> DeleteBooth(int id)
        {
            var isExist = await _boothService.GetBooth(id);
            if (isExist == null)
            {
                return NotFound();
            }
            var booth = await _boothService.DeleteBooth(id);
            return Ok(booth);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetBoothCount()
        {
            var count = await _boothService.GetBoothCount();
            return Ok(count);
        }
    }
}
