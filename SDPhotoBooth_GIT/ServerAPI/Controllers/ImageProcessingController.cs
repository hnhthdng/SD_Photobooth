using BusinessLogic.DTO.ImageProcessingDTO;
using BusinessLogic.Service.IService;
using BusinessLogic.Utils;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Services;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageProcessingController : ControllerBase
    {
        private readonly RabbitMqService _rabbitService;
        private readonly FirebaseService _firebaseService;
        private readonly IPhotoStyleService _photoStyleService;
        private readonly ISessionService _sessionService;

        public ImageProcessingController(RabbitMqService rabbitService, FirebaseService firebaseService, IPhotoStyleService photoStyleService, ISessionService sessionService)
        {
            _rabbitService = rabbitService;
            _firebaseService = firebaseService;
            _photoStyleService = photoStyleService;
            _sessionService = sessionService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendImageProcessing([FromForm] ImageProcessingDTO request)
        {
            if (request.image == null || request.image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

            if (string.IsNullOrEmpty(request.connectionId) || string.IsNullOrEmpty(request.sessionCode))
            {
                return BadRequest("Connection ID and session code are required.");
            }

            var photoStyle = await _photoStyleService.GetPhotoStyleById(request.photoStyleId);

            if (photoStyle == null)
            {
                return BadRequest("Photo style not found.");
            }

            var session = await _sessionService.UpdateSessionAbleTaken(request.sessionCode);

            if (session == null)
            {
                return BadRequest("Session not found or already taken.");
            }

            string firebaseUrl;
            using (var stream = request.image.OpenReadStream())
            {
                firebaseUrl = await _firebaseService.UploadImageAsync(stream, request.image.FileName, request.image.ContentType);
            }

            //string queueName = QueueRoutingHelper.GetQueueName(photoStyle.Controlnets);

            await _rabbitService.SendMessageAsync(firebaseUrl, request.connectionId, photoStyle, request.sessionCode);

            return Ok(new { Message = "Image sent for processing", ImageUrl = firebaseUrl });
        }
    }
}
