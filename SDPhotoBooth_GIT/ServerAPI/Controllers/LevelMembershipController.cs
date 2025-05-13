using AutoMapper;
using BusinessLogic.DTO.LevelMembershipDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelMembershipController : ControllerBase
    {
        private readonly ILevelMembershipService _levelMembershipService;
        private readonly IMapper _mapper;
        public LevelMembershipController(ILevelMembershipService levelMembershipService, IMapper mapper)
        {
            _levelMembershipService = levelMembershipService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetLevelMemberships([FromQuery] PaginationParams pagination)
        {
            try
            {
                var levelMemberships = await _levelMembershipService.GetAllLevelMemberships(pagination);
                return Ok(levelMemberships);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetLevelMembership(int id)
        {
            var levelMembership = await _levelMembershipService.GetLevelMembershipById(id);
            if (levelMembership == null)
            {
                return NotFound();
            }
            return Ok(levelMembership);
        }

    
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLevelMembership([FromBody] CreateLevelMembershipRequestDTO levelMembershipRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (levelMembershipRequestDTO.Point < 0)
            {
                return BadRequest("Point cannot be negative.");
            }
            bool isNextLevelAvailable = levelMembershipRequestDTO.NextLevelId.HasValue;
            if (isNextLevelAvailable)
            {
                var nextLevel = await _levelMembershipService.GetLevelMembershipById(levelMembershipRequestDTO.NextLevelId.Value);
                if (nextLevel == null)
                {
                    return NotFound($"Next Level Membership with ID {levelMembershipRequestDTO.NextLevelId} not found.");
                }
                // Check if the next level membership already exists
                var allLevelMemberships = await _levelMembershipService.GetAllLevelMemberships(null);
                var nextLevelMembership = allLevelMemberships.FirstOrDefault(l => l.NextLevelId == levelMembershipRequestDTO.NextLevelId);
                if (nextLevelMembership != null)
                {
                    return BadRequest($"Next Level Membership with ID {levelMembershipRequestDTO.NextLevelId} already exists.");
                }
            }
            var createdLevelMembership = await _levelMembershipService.CreateLevelMembership(levelMembershipRequestDTO);

            return Ok(createdLevelMembership);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLevelMembership(int id, [FromBody] UpdateLevelMembershipRequestDTO levelMembershipRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingLevelMembership = await _levelMembershipService.GetLevelMembershipById(id);
            if (existingLevelMembership == null)
            {
                return NotFound();
            }
            if (levelMembershipRequestDTO.Point < 0)
            {
                return BadRequest("Point cannot be negative.");
            }

            bool isNextLevelAvailable = levelMembershipRequestDTO.NextLevelId.HasValue;
            if (isNextLevelAvailable)
            {
                var nextLevel = await _levelMembershipService.GetLevelMembershipById(levelMembershipRequestDTO.NextLevelId.Value);
                if (nextLevel == null)
                {
                    return NotFound($"Next Level Membership with ID {levelMembershipRequestDTO.NextLevelId} not found.");
                }
                // Check if the next level membership already exists
                var allLevelMemberships = await _levelMembershipService.GetAllLevelMemberships(null);
                var nextLevelMembership = allLevelMemberships.FirstOrDefault(l => l.NextLevelId == levelMembershipRequestDTO.NextLevelId);
                if (nextLevelMembership != null && nextLevelMembership.Id != id)
                {
                    return BadRequest($"Next Level Membership with ID {levelMembershipRequestDTO.NextLevelId} already exists.");
                }
            }

            var updatedLevelMembership = await _levelMembershipService.UpdateLevelMembership(id, levelMembershipRequestDTO);
            return Ok(updatedLevelMembership);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLevelMembership(int id)
        {
            var result = await _levelMembershipService.DeleteLevelMembership(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _levelMembershipService.GetCount();
            return Ok(count);
        }
    }
}
