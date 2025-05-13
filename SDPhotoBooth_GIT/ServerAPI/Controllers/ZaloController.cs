using BusinessLogic.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Utilities;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/zalo")]
    [AllowAnonymous]
    public class ZaloController : ControllerBase
    {
        private readonly IPhotoHistoryService photoHistoryService;

        public ZaloController(IPhotoHistoryService photoHistoryService, IUserService userService)
        {
            this.photoHistoryService = photoHistoryService;
        }

        [HttpGet("send-photobooth-via-zalo")]
        public async Task<IActionResult> SendPhotoBoothViaZalo([FromQuery] string sessionCode)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionCode))
                {
                    return BadRequest("Phone and sessionCode are required.");
                }

                var photoUrls = (await photoHistoryService.GetListPhotosFromSessionCode(sessionCode)).TakeLast(48).ToArray();

                string photoUrlsString = string.Join("\n", await UrlShortenerHelper.ShortenUrlsParallel(photoUrls));

                return Ok(new { photoUrls = photoUrlsString });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error occured!", error = ex.Message });
            }
        }
    }
}