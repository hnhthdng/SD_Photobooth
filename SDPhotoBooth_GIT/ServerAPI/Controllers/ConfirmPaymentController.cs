using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServerAPI.Hubs;

namespace ServerAPI.Controllers
{
    [ApiController]
    public class ConfirmPaymentController : ControllerBase
    {
        private readonly IHubContext<TranferPhotoHub> _hubContext;

        public ConfirmPaymentController(IHubContext<TranferPhotoHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("confirm-payment-payos")]
        public async Task ConfirmPaymentPayos([FromQuery] string connectId, [FromQuery] string code, [FromQuery] string id, [FromQuery] bool cancel, [FromQuery] string status, [FromQuery] long orderCode)
        {
            await _hubContext.Clients.Client(connectId).SendAsync("ConfirmPaymentPayOS", cancel, status, orderCode);
        }
    }
}