using BusinessLogic.DTO.PaymentMethod;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet("count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _paymentMethodService.GetCount();
            return Ok(count);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get([FromQuery] PaginationParams pagination)
        {
            try
            {
                var paymentMethods = await _paymentMethodService.GetAllPaymentMethod(pagination);
                return Ok(paymentMethods);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("all/destop")]
        public async Task<IActionResult> GetAllForDestop()
        {
            var paymentMethods = await _paymentMethodService.GetAllFor(false, true);
            return Ok(paymentMethods);
        }

        [HttpGet("all/mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllForMobile()
        {
            var paymentMethods = await _paymentMethodService.GetAllFor(true, null);
            return Ok(paymentMethods);
        }

        [HttpGet("all/web")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<IActionResult> GetAllForWeb()
        {
            var paymentMethods = await _paymentMethodService.GetAllFor(false, null);
            return Ok(paymentMethods);
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethod(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            return Ok(paymentMethod);
        }

        [HttpGet("by-name/{paymentMethodName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(string paymentMethodName)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethod(paymentMethodName);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            return Ok(paymentMethod);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] PaymentMethodRequestDTO paymentMethodRequestDTO)
        {
            if (paymentMethodRequestDTO == null)
            {
                return BadRequest("Invalid data provided.");
            }
            var paymentMethod = await _paymentMethodService.CreatePaymentMethod(paymentMethodRequestDTO);
            return Ok(paymentMethod);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] PaymentMethodRequestDTO paymentMethodRequestDTO)
        {
            if (paymentMethodRequestDTO == null)
            {
                return BadRequest("Invalid data provided.");
            }
            var paymentMethod = await _paymentMethodService.UpdatePaymentMethod(id, paymentMethodRequestDTO);
            return Ok(paymentMethod);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid data provided.");
            }
            var paymentMethod = await _paymentMethodService.GetPaymentMethod(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }
            var deletedPaymentMethod = await _paymentMethodService.DeletePaymentMethod(id);
            return Ok(deletedPaymentMethod);
        }

    }
}
