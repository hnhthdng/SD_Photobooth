using AutoMapper;
using BusinessLogic.DTO.UserDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Enums;
using BussinessObject.Models;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ILocationService _locationService;
        public UserController(IUserService userService, IMapper mapper, ILocationService locationService, UserManager<User> userManager)
        {
            _userService = userService;
            _locationService = locationService;
            _userManager = userManager;
        }

        [HttpGet("manager")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllManagers([FromQuery] PaginationParams? filter)
        {
            var (data, totalCount) = await _userService.GetAllManager(filter);
            return Ok(new
            {
                Data = data,
                TotalCount = totalCount
            });
        }

        [HttpGet("staff")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetAllStaffs([FromQuery] PaginationParams? filter)
        {
            var (data, totalCount) = await _userService.GetAllStaff(filter);
            return Ok(new
            {
                Data = data,
                TotalCount = totalCount
            });
        }

        [HttpGet("customer")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] PaginationParams? filter)
        {
            var (data, totalCount) = await _userService.GetAllCustomer(filter);
            return Ok(new
            {
                Data = data,
                TotalCount = totalCount
            });
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> Create([FromBody] UserRequestDTO userRequestDTO)
        {
            var user = await _userService.CreateUser(userRequestDTO);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPost("update-ban-status")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateBanStatus([FromQuery] string email, [FromQuery] bool isBanned)
        {
            var requester = await _userManager.GetUserAsync(User);
            var requesterRole = (await _userManager.GetRolesAsync(requester)).FirstOrDefault();

            var targetUser = await _userService.UserDetail(email);
            if (targetUser == null)
                return NotFound("User not found.");

            var targetRole = targetUser.Role;

            // Manager can only ban/unban Customer
            if (requesterRole == "Manager" && targetRole == "Customer")
            {
                var result = isBanned
                    ? await _userService.BanUser(email)
                    : await _userService.UnbanUser(email);
                return Ok(result);
            }

            // Admin can ban/unban Manager or Staff
            if (requesterRole == "Admin" && (targetRole == "Manager" || targetRole == "Staff"))
            {
                var result = isBanned
                    ? await _userService.BanUser(email)
                    : await _userService.UnbanUser(email);
                return Ok(result);
            }

            return Forbid(); 
        }



        [HttpGet("detail")]
        [Authorize(Roles = "Admin, Manager, Staff")]
        public async Task<IActionResult> UserDetail([FromQuery] string email)
        {
            var requester = await _userManager.GetUserAsync(User);
            var requesterRole = (await _userManager.GetRolesAsync(requester)).FirstOrDefault();

            var targetUser = await _userService.UserDetail(email);
            if (targetUser == null)
                return NotFound("User not found.");

            var targetRole = targetUser.Role;

            // Staff can only view Customer details
            if (requesterRole == "Staff" && targetRole == "Customer")
            {
                return Ok(targetUser);
            }
            // Manager can view Customer and Staff details
            else if (requesterRole == "Manager" && (targetRole == "Customer" || targetRole == "Staff"))
            {
                return Ok(targetUser);
            }
            // Admin can only view Manager and Staff details
            else if (requesterRole == "Admin" && (targetRole == "Manager" || targetRole == "Staff"))
            {
                return Ok(targetUser);
            }
            return Forbid();
        }


        [HttpPost("change-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole([FromQuery] string email, [FromQuery] UserType role)
        {
            var isExistUser = await _userService.UserDetail(email);
            if (isExistUser == null)
            {
                return NotFound();
            }
            if (role == UserType.Customer || role == UserType.Admin)
            {
                return BadRequest("Role cannot be Customer");
            }
            var isExistRole = isExistUser.Role;
            if (isExistRole == "Manager" || isExistRole == "Staff")
            {
                var result = await _userService.ChangeRole(email, role);
                return Ok(result);
            }
            else
            {
                return BadRequest("Role cannot be changed");
            }
        }

        [HttpPost("staff/move-location")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> MoveLocation([FromQuery] string email, [FromQuery] int locationId)
        {
            var isExistUser = await _userService.UserDetail(email);
            if (isExistUser == null)
            {
                return NotFound();
            }
            if(isExistUser.Role != "Staff")
            {
                return BadRequest("User is not staff");
            }
            var isExistLocation = await _locationService.GetLocation(locationId);
            if (isExistLocation == null)
            {
                return BadRequest("Location is not found");
            }
            var result = await _userService.MoveLocation(email, locationId);
            return Ok(result);
        }

    }
}
