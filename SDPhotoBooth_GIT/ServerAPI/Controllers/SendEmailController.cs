using BusinessLogic.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IPhotoHistoryService _photoHistoryService;
        private readonly IOrderService _orderService;
        public SendEmailController(IEmailService emailService, IPhotoHistoryService photoHistoryService, IOrderService orderService)
        {
            _emailService = emailService;
            _photoHistoryService = photoHistoryService;
            _orderService = orderService;
        }

        [HttpPost("send/{sessionCode}")]
        public async Task<IActionResult> SendEmail(string sessionCode)
        {
            var photos = await _photoHistoryService.GetListPhotosFromSessionCode(sessionCode);
            if (photos == null || photos.Length == 0)
            {
                return NotFound("Photos of this session not found.");
            }

            var order = await _orderService.GetOrderBySessionCode(sessionCode);
            if (order == null || string.IsNullOrEmpty(order.Email))
            {
                return NotFound("Email not found");
            }

            var result = await _emailService.SendEmailAsync(order.Email, photos);
            if (result)
                return Ok(new { message = "Send email successfully.", emailSentTo = order.Email });
            else
                return StatusCode(500, new { message = "Send email failed.", emailSentTo = order.Email });
        }

    }
}
