using AutoMapper;
using BusinessLogic.DTO.CoordinateDTO;
using BusinessLogic.DTO.FrameDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameController : ControllerBase
    {
        private readonly IFrameService _frameService;
        private readonly IFrameStyleService _frameStyleService;
        private readonly ICoordinateService _coordinateService;
        private readonly IMapper _mapper;
        private readonly FirebaseService _firebaseService;

        public FrameController(IFrameService frameService, IMapper mapper, IFrameStyleService frameStyleService,
            FirebaseService firebaseService, ICoordinateService coordinateService)
        {
            _frameService = frameService;
            _mapper = mapper;
            _frameStyleService = frameStyleService;
            _firebaseService = firebaseService;
            _coordinateService = coordinateService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateFrame([FromForm] FrameWithFormFileDTO form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (form.FrameFile == null || form.FrameFile.Length == 0)
            {
                return BadRequest("Invalid upload file data");
            }
            if (!form.SlotCount.HasValue)
            {
                return BadRequest("Invalid Slot Count");
            }
            if (!form.FrameStyleId.HasValue)
            {
                return BadRequest("Invalid Frame Style Id");
            }
            var frameStyle = await _frameStyleService.GetFrameStyle(form.FrameStyleId.Value);
            if (frameStyle == null)
            {
                return BadRequest("Invalid Frame Style");
            }
            if(!form.ForMobile.HasValue) form.ForMobile = false;

            if (form.CoordinateDTOs == null || !form.CoordinateDTOs.Any())
            {
                return BadRequest("Coordinates must not be null or empty.");
            }

            var uploadedUrl = await _firebaseService.UploadImageAsync(
            form.FrameFile.OpenReadStream(),
            form.FrameFile.FileName,
            form.FrameFile.ContentType);

            var frameRequestDTO = _mapper.Map<FrameRequestDTO>(form);
            frameRequestDTO.FrameUrl = uploadedUrl;

            var frame = await _frameService.CreateFrame(frameRequestDTO);
            foreach (var coordinate in form.CoordinateDTOs)
            {
                var coordinateRequestDTO = new CoordinateRequestDTO
                {
                    FrameId = frame.Id,
                    X = coordinate.X,
                    Y = coordinate.Y,
                    Width = coordinate.Width,
                    Height = coordinate.Height
                };
                await _coordinateService.CreateCoordinate(coordinateRequestDTO);
            }
            return Ok(frame);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFrame(int id)
        {
            var isDeleted = await _frameService.DeleteFrame(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFrame(int id)
        {
            var frame = await _frameService.GetFrame(id);
            if (frame == null)
            {
                return NotFound();
            }
            return Ok(frame);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFrames([FromQuery] PaginationParams pagination)
        {
            try
            {
                var frames = await _frameService.GetFrames(pagination);
                return Ok(frames);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("frame-style/{frameStyleId:int}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetFrameByFrameStyleId(int frameStyleId)
        {
            var frames = await _frameService.GetFrameByFrameStyleId(frameStyleId);
            return Ok(frames);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateFrame(int id, [FromForm] FrameWithFormFileDTO form)
        {
            var frameRequestDTO = _mapper.Map<FrameRequestDTO>(form);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isExists = await _frameService.GetFrame(id);
            if (isExists == null)
            {
                return NotFound();
            }
            if(form.FrameStyleId.HasValue)
            {
                var frameStyle = await _frameStyleService.GetFrameStyle(form.FrameStyleId.Value);
                if (frameStyle == null)
                {
                    return BadRequest("Invalid Frame Style");
                }
            }
            if(form.SlotCount.HasValue && form.SlotCount.Value <= 0) return BadRequest("Invalid SlotCount");

            if (form.FrameFile != null && form.FrameFile.Length != 0)
            {
                var uploadedUrl = await _firebaseService.UploadImageAsync(
                    form.FrameFile.OpenReadStream(),
                    form.FrameFile.FileName,
                    form.FrameFile.ContentType);

                frameRequestDTO.FrameUrl = uploadedUrl; 
            }

            var frame = await _frameService.UpdateFrame(id, frameRequestDTO);
            return Ok(frame);
        }

        [HttpGet("by-name/{frameName}")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> SearchFrame(string frameName)
        {
            var frames = await _frameService.SearchFrame(frameName);
            return Ok(frames);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _frameService.GetCount();
            return Ok(count);
        }
    }
}
