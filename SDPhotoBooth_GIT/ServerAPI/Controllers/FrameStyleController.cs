using BusinessLogic.DTO.FrameDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FrameStyleController : ControllerBase
    {
        private readonly IFrameStyleService _frameStyleService;
        private readonly FirebaseService _firebaseService;
        public FrameStyleController(IFrameStyleService frameStyleService, FirebaseService firebaseService)
        {
            _frameStyleService = frameStyleService;
            _firebaseService = firebaseService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateFrameStyle([FromForm] FrameStyleWithFormFileDTO form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? imageUrl = null;

            if (form.Image != null && form.Image.Length > 0)
            {
                imageUrl = await _firebaseService.UploadImageAsync(
                    form.Image.OpenReadStream(),
                    form.Image.FileName,
                    form.Image.ContentType);
            }

            var dto = new FrameStyleRequestDTO
            {
                Name = form.Name,
                Description = form.Description,
                ImageUrl = imageUrl
            };

            var created = await _frameStyleService.CreateFrameStyle(dto);
            return Ok(created);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFrameStyle(int id)
        {
            var isExist = await _frameStyleService.GetFrameStyle(id);
            if (isExist == null)
            {
                return NotFound();
            }
            var result = await _frameStyleService.DeleteFrameStyle(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetFrameStyles([FromQuery] PaginationParams pagination)
        {
            try
            {
                var frameStyles = await _frameStyleService.GetFrameStyles(pagination);
                return Ok(frameStyles);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetFrameStyle(int id)
        {
            var frameStyle = await _frameStyleService.GetFrameStyle(id);
            if (frameStyle == null)
            {
                return NotFound();
            }
            return Ok(frameStyle);
        }

        [HttpGet("by-name/{frameStyleName}")]
        public async Task<IActionResult> SearchFrameStyle(string frameStyleName)
        {
            var frameStyles = await _frameStyleService.SearchFrameStyle(frameStyleName);
            return Ok(frameStyles);
        }

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateFrameStyle(int id, [FromForm] FrameStyleWithFormFileDTO form)
        {
            var existing = await _frameStyleService.GetFrameStyle(id);
            if (existing == null)
                return NotFound("Frame style not found.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? imageUrl = existing.ImageUrl;

            if (form.Image != null && form.Image.Length > 0)
            {
                imageUrl = await _firebaseService.UploadImageAsync(
                    form.Image.OpenReadStream(),
                    form.Image.FileName,
                    form.Image.ContentType);
            }

            var dto = new FrameStyleRequestDTO
            {
                Name = form.Name,
                Description = form.Description,
                ImageUrl = imageUrl
            };

            var result = await _frameStyleService.UpdateFrameStyle(id, dto);
            return Ok(result);
        }


        [HttpGet("count")]
        public async Task<IActionResult> GetFrameStyleCount()
        {
            var count = await _frameStyleService.GetCount();
            return Ok(count);
        }
    }
}
