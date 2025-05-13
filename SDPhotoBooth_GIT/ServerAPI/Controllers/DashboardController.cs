using BusinessLogic.DTO.DashboardDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IUserService userService;
        private readonly IDepositService depositService;
        private readonly ITransactionService transactionService;
        private readonly ISessionService sessionService;

        public DashboardController(IOrderService orderService, IUserService userService, IDepositService depositService, ITransactionService transactionService, ISessionService sessionService)
        {
            this.orderService = orderService;
            this.userService = userService;
            this.depositService = depositService;
            this.transactionService = transactionService;
            this.sessionService = sessionService;
        }

        [HttpGet("statictis-order")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetStaticOrder([FromQuery] StaticType staticType = StaticType.Day)
        {
            var totalOrderStatic = await orderService.StaticOrderCreated(staticType);

            return Ok(totalOrderStatic);
        }

        [HttpGet("statictis-deposit")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetStaticDeposit([FromQuery] StaticType staticType = StaticType.Day)
        {
            var totalDepositStatic = await depositService.StaticDepositCreated(staticType);

            return Ok(totalDepositStatic);
        }

        [HttpGet("statictis-user")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetStaticUser([FromQuery] StaticType staticType = StaticType.Day)
        {
            var totalUserStatic = await userService.StaticUserCreated(staticType);

            return Ok(totalUserStatic);
        }

        [HttpGet("statictis-revenue")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetStaticRevenue([FromQuery] StaticType staticType = StaticType.Day)
        {
            var totalRevenueStatic = await transactionService.StaticRevenue(staticType);

            return Ok(totalRevenueStatic);
        }

        [HttpGet("statictis-revenue-by-platform-type")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetStaticRevenueByPlatformType([FromQuery] GroupingType staticType = GroupingType.Day)
        {
            var totalTransactionStatic = await transactionService.StaticRevenueByPlatformType(staticType);
            return Ok(totalTransactionStatic);
        }

        [HttpGet("statictis-usage-channel")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetStaticUsageChannel([FromQuery] UsageChannelFilterDTO filter)
        {
            if (filter.ChannelGroupingType == ChannelGroupingType.Booth && filter.LocationId == null)
            {
                return BadRequest("LocationId is required when channelGroupingType is Booth");
            }

            var totalUsageChannelStatic = await sessionService.StaticUsageChannel(filter);
            return Ok(totalUsageChannelStatic);
        }

        [HttpGet("statictis-revenue-staffs")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetStaticRevenueStaffs([FromQuery] GroupingType staticType = GroupingType.Day)
        {
            var totalRevenueStaffsStatic = await userService.StaticRevenueStaffs(staticType);
            return Ok(totalRevenueStaffsStatic);
        }

        [HttpGet("statictis-revenue-own")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> GetStaticRevenue([FromQuery] RevenueFilterDTO revenueFilterDTO)
        {
            if(revenueFilterDTO.StaffId != null)
            {
                var role = User.FindFirstValue(ClaimTypes.Role);
                if (role != "Manager")
                {
                    return BadRequest("You are not allowed to get revenue of other staff");
                }
            }
            else revenueFilterDTO.StaffId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var totalRevenueStaffStatic = await userService.StaticRevenue(revenueFilterDTO);
            return Ok(totalRevenueStaffStatic);
        }
    }
}
