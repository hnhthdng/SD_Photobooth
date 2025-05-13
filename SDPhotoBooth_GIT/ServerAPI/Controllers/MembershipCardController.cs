using AutoMapper;
using BusinessLogic.DTO.MembershipCardDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipCardController : ControllerBase
    {
        private readonly IMembershipCardService _membershipCardService;
        private readonly IUserService _userService;
        private readonly ILevelMembershipService _levelMembershipService;
        public MembershipCardController(IMembershipCardService membershipCardService,IUserService userService, ILevelMembershipService levelMembershipService)
        {
            _membershipCardService = membershipCardService;
            _levelMembershipService = levelMembershipService;
            _userService = userService;
        }


        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllMembershipCards([FromQuery] PaginationParams pagination)
        {
            try
            {
                var result = await _membershipCardService.GetAll(pagination);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetById(int id)
        {
            var membershipCard = await _membershipCardService.GetById(id);
            if (membershipCard == null)
            {
                return NotFound();
            }
            return Ok(membershipCard);
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var membershipCard = await _membershipCardService.GetByCustomerId(customerId);
            if (membershipCard == null)
            {
                return NotFound();
            }
            return Ok(membershipCard);
        }

        [HttpGet("mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetForCus()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var membershipCard = await _membershipCardService.GetByCustomerId(userId);
            if (membershipCard == null)
            {
                membershipCard = await _membershipCardService.Create(new CreateMembershipCardRequestDTO()
                {
                    CustomerId = userId,
                    LevelMemberShipId = 1,
                    Points = 0,
                    IsActive = true
                });
            }
            return Ok(membershipCard);
        }

        [HttpGet("level/{levelMembershipId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetByLevelMembershipId(int levelMembershipId)
        {
            var membershipCards = await _membershipCardService.GetByLevelMembershipId(levelMembershipId);
            if (membershipCards == null || !membershipCards.Any())
            {
                return NotFound();
            }
            return Ok(membershipCards);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMembershipCardRequestDTO dto)
        {
            var existingCard = await _membershipCardService.GetById(id);
            if (existingCard == null)
                return NotFound();

            if (dto.LevelMemberShipId.HasValue)
            {
                var level = await _levelMembershipService.GetLevelMembershipById(dto.LevelMemberShipId.Value);
                if (level == null)
                    return NotFound($"Level Membership with ID {dto.LevelMemberShipId} not found.");
            }

            if (dto.Points.HasValue && dto.Points < 0)
            {
                return BadRequest("Points cannot be negative.");
            }

            var updatedCard = await _membershipCardService.Update(id, dto);
            return Ok(updatedCard);
        }

        [HttpPut("upgrade-level")]
        [Authorize(Roles ="Staff")]
        public async Task<IActionResult> UpgradeLevel([FromBody] UpgradeMembershipCardRequestDTO upgradeMembershipCard)
        {
            var user = await _userService.UserDetail(upgradeMembershipCard.email);
            if (user == null)
                return NotFound("User not found.");

            var existingCard = await _membershipCardService.GetById(user.MembershipCard.Id);
            if (existingCard == null)
                return NotFound();

            if (!existingCard.IsActive)
                return BadRequest("Cannot upgrade. Membership card is not active.");

            bool isNextLevelAvailable = existingCard.LevelMemberShip.NextLevelId.HasValue;
            if (!isNextLevelAvailable)
            {
                return BadRequest("Cannot upgrade. No next level available.");
            }
            var nextLevel = await _levelMembershipService.GetLevelMembershipById(existingCard.LevelMemberShip.NextLevelId.Value);

            if (existingCard.Points < nextLevel.Point)
                return BadRequest($"Insufficient points ({existingCard.Points}) to upgrade to level {nextLevel.Name} (requires {existingCard.LevelMemberShip.Name}).");

            var dto = new UpdateMembershipCardRequestDTO
            {
                LevelMemberShipId = nextLevel.Id,
                Points = existingCard.Points - nextLevel.Point
            };

            var updatedCard = await _membershipCardService.Update(existingCard.Id, dto);
            return Ok(updatedCard);
        }

        [HttpPut("mobile/upgrade-level")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpgradeLevel()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var existingCard = await _membershipCardService.GetByCustomerId(userId);
            if (existingCard == null)
                return NotFound("Membership card not found.");

            if (!existingCard.IsActive)
                return BadRequest("Cannot upgrade. Membership card is not active.");

            bool isNextLevelAvailable = existingCard.LevelMemberShip.NextLevelId.HasValue;
            if (!isNextLevelAvailable)
            {
                return BadRequest("Cannot upgrade. No next level available.");
            }
            var nextLevel = await _levelMembershipService.GetLevelMembershipById(existingCard.LevelMemberShip.NextLevelId.Value);

            if (existingCard.Points < nextLevel.Point)
                return BadRequest($"Insufficient points ({existingCard.Points}) to upgrade to level {nextLevel.Name} (requires {existingCard.LevelMemberShip.Name}).");

            var dto = new UpdateMembershipCardRequestDTO
            {
                LevelMemberShipId = nextLevel.Id,
                Points = existingCard.Points - nextLevel.Point
            };

            var updatedCard = await _membershipCardService.Update(existingCard.Id, dto);
            return Ok(updatedCard);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _membershipCardService.GetCount();
            return Ok(count);
        }
    }
}
