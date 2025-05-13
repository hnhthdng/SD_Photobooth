using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllPayments([FromQuery] PaginationParams pagination)
        {
            try
            {
                var payments = await _paymentService.GetAllPayments(pagination);
                return Ok(payments);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetPayment()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var payments = await _paymentService.GetPaymentByCusId(userId);
            
            return Ok(payments);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _paymentService.GetCount();
            return Ok(count);
        }

    }
}
