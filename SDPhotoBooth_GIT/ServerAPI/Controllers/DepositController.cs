using BusinessLogic.DTO.DepositDTO;
using BusinessLogic.DTO.PaymentDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService _depositService;
        private readonly IPaymentService paymentService;
        private readonly IDepositProductService depositProductService;

        public DepositController(IDepositService depositService, IPaymentService paymentService, IDepositProductService depositProductService)
        {
            _depositService = depositService;
            this.paymentService = paymentService;
            this.depositProductService = depositProductService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateDeposit([FromBody] DepositRequestDTO depositRequest)
        {
            ModelState.Remove("Amount");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not found");
                }

                var depositProduct = await depositProductService.GetByIdAsync(depositRequest.DepositProductId);

                if (depositProduct == null)
                {
                    return null;
                }

                depositRequest.Amount = depositProduct.Price + (depositProduct.AmountAdd ?? 0m);

                var deposit = await _depositService.CreateDeposit(depositRequest, userId);

                var payment = await paymentService.CreatePaymentForDeposit(new PaymentRequestDTO
                {
                    DepositId = deposit.Id,
                    Amount = deposit.Amount,
                    PaymentMethodId = depositRequest.PaymentMethodId
                });

                deposit.PaymentId = payment.Id;
                return Ok(deposit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Authorize(Roles ="Manager")]
        public async Task<IActionResult> GetDeposits([FromQuery] PaginationParams pagination)
        {
            try
            {
                var result = await _depositService.GetAllDeposits(pagination);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("count")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _depositService.GetCount();
            return Ok(count);
        }

        [HttpGet("mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetDepositsByCusId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User not found");
            }
            try
            {
                var result = await _depositService.GetDepositsByCusId(userId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}