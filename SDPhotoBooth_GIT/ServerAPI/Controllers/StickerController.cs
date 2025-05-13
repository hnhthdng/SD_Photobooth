using BusinessLogic.DTO.StickerDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StickerController : ControllerBase
    {
        private readonly IStickerService _stickerService;
        private readonly IStickerStyleService _stickerStyleService;
        private readonly FirebaseService _firebaseService;
        public StickerController(IStickerService stickerService, IStickerStyleService stickerStyleService, FirebaseService firebaseService)
        {
            _stickerService = stickerService;
            _stickerStyleService = stickerStyleService;
            _firebaseService = firebaseService;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _stickerService.GetCount();
            return Ok(count);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<StickerResponseDTO>> CreateSticker([FromForm] StickerWithFormFileRequestDTO stickerRequestDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (stickerRequestDTO.StickerFile == null || stickerRequestDTO.StickerFile.Length == 0)
                return BadRequest("Sticker file is required.");

            if (stickerRequestDTO.StickerStyleId.HasValue)
            {
                var stickerStyle = await _stickerStyleService.GetStickerStyleById(stickerRequestDTO.StickerStyleId.Value);
                if (stickerStyle == null)
                {
                    return BadRequest("Sticker Style does not exist");
                }
            }
            string uploadedUrl = await _firebaseService.UploadImageAsync(stickerRequestDTO.StickerFile.OpenReadStream(), stickerRequestDTO.StickerFile.FileName, stickerRequestDTO.StickerFile.ContentType);
            
            var dto = new StickerRequestDTO
            {
                Name = stickerRequestDTO.Name,
                StickerUrl = uploadedUrl,
                StickerStyleId = stickerRequestDTO.StickerStyleId
            };
            var sticker = await _stickerService.CreateSticker(dto);
            return Ok(sticker);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<StickerResponseDTO>> DeleteSticker(int id)
        {
            var sticker = await _stickerService.GetStickerById(id);
            if (sticker == null)
            {
                return NotFound();
            }
            var deletedSticker = await _stickerService.DeleteSticker(id);
            return Ok(deletedSticker);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StickerResponseDTO>>> GetAllStickers([FromQuery] PaginationParams pagination)
        {
            try
            {
                var stickers = await _stickerService.GetAllStickers(pagination);
                return Ok(stickers);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}")] 
        public async Task<ActionResult<StickerResponseDTO>> GetSticker(int id)
        {
            var sticker = await _stickerService.GetStickerById(id);
            if (sticker == null)
            {
                return NotFound();
            }
            return Ok(sticker);
        }

        [HttpGet("by-style/{styleId}")]
        public async Task<ActionResult<IEnumerable<StickerResponseDTO>>> GetStickerByStyle(int styleId)
        {
            var stickerStyle = await _stickerStyleService.GetStickerStyleById(styleId);
            if (stickerStyle == null)
            {
                return BadRequest("Sticker Style does not exist");
            }
            var stickers = await _stickerService.GetStickerByStyleId(styleId);
            return Ok(stickers);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<StickerResponseDTO>> UpdateSticker(int id, [FromForm] StickerWithFormFileRequestDTO stickerRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sticker = await _stickerService.GetStickerById(id);
            if (sticker == null)
            {
                return NotFound();
            }

            if (stickerRequestDTO.StickerStyleId.HasValue)
            {
                var stickerStyle = await _stickerStyleService.GetStickerStyleById(stickerRequestDTO.StickerStyleId.Value);
                if (stickerStyle == null)
                {
                    return BadRequest("Sticker Style does not exist");
                }
            }

            StickerRequestDTO dto = new StickerRequestDTO
            {
                Name = stickerRequestDTO.Name,
                StickerStyleId = stickerRequestDTO.StickerStyleId
            };

            if (stickerRequestDTO.StickerFile != null && stickerRequestDTO.StickerFile.Length != 0)
            {
                string uploadedUrl = await _firebaseService.UploadImageAsync(stickerRequestDTO.StickerFile.OpenReadStream(), stickerRequestDTO.StickerFile.FileName, stickerRequestDTO.StickerFile.ContentType);
                dto.StickerUrl = uploadedUrl;
            }
            var updatedSticker = await _stickerService.UpdateSticker(id, dto);
            return Ok(updatedSticker);
        }
    }
}
