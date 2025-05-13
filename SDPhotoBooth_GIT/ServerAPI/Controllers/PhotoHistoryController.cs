using BusinessLogic.DTO.PhotoHistoryDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ServerAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotoHistoryController : ControllerBase
    {
        private readonly IPhotoHistoryService _photoHistoryService;
        private readonly FirebaseService _firebaseService;

        public PhotoHistoryController(IPhotoHistoryService photoHistoryService, FirebaseService firebaseService)
        {
            _photoHistoryService = photoHistoryService;
            _firebaseService = firebaseService;
        }


        [HttpGet("{sessionCode}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Get(string sessionCode)
        {
            var photos = await _photoHistoryService.GetListPhotosFromSessionCode(sessionCode);
            if (photos == null)
            {
                return NotFound();
            }
            return Ok(photos);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var photoHistory = await _photoHistoryService.GetPhotoHistoryAsync(id, userId);
            if (photoHistory == null)
            {
                return NotFound();
            }
            return Ok(photoHistory);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetListPhotoHistoryAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var photoHistories = await _photoHistoryService.GetListPhotoHistoryAsync(userId);
            return Ok(photoHistories);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _photoHistoryService.GetCount();
            return Ok(count);
        }

        [HttpGet("manage/all")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllPhotoHistory([FromQuery] PaginationParams pagination)
        {
            try
            {
                var photoHistories = await _photoHistoryService.GetAllPhotoHistory(pagination);
                var totalCount = await _photoHistoryService.GetCount();

                return Ok(new
                {
                    Data = photoHistories,
                    TotalCount = totalCount
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("manage/customer/{customerId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetPhotoHistoryByCustomerIdAsync(string customerId)
        {
            var photoHistories = await _photoHistoryService.GetPhotoHistoryByCustomerIdAsync(customerId);
            return Ok(photoHistories);
        }

        [HttpGet("manage/booth/{boothId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetPhotoHistoryByBoothIdAsync(int boothId)
        {
            var photoHistories = await _photoHistoryService.GetPhotoHistoryByBoothIdAsync(boothId);
            return Ok(photoHistories);
        }

        [HttpGet("manage/location/{locationId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetPhotoHistoryByLocationIdAsync(int locationId)
        {
            var photoHistories = await _photoHistoryService.GetPhotoHistoryByLocationIdAsync(locationId);
            return Ok(photoHistories);
        }

        [HttpGet("manage/session/{sessionCode}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetPhotoHistoryBySessionCodeAsync(string sessionCode)
        {
            var photoHistory = await _photoHistoryService.GetPhotoHistoryBySessionCodeAsync(sessionCode);
            if (photoHistory == null)
            {
                return NotFound();
            }
            return Ok(photoHistory);
        }


        [HttpGet("manage/photo/{photoHistoryId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetPhotoHistoryByIdAsync(int photoHistoryId)
        {
            var photoHistory = await _photoHistoryService.GetPhotoHistoryByIdAsync(photoHistoryId);
            if (photoHistory == null)
            {
                return NotFound();
            }
            return Ok(photoHistory);
        }

        [HttpPost("upload")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadPhotos([FromForm] PhotoHistoryRequestDTO request)
        {
            if (request.PhotoPaths == null || !request.PhotoPaths.Any() || string.IsNullOrEmpty(request.SessionCode))
            {
                return BadRequest("Invalid request data.");
            }

            var uploadedUrls = new List<string>();

            foreach (var image in request.PhotoPaths)
            {
                if (image.Length == 0) continue;

                string fileName = $"{request.SessionCode}/{image.FileName}";
                await using var stream = image.OpenReadStream();
                string contentType = image.ContentType;

                string url = await _firebaseService.UploadIfNotExistsAsync(stream, fileName, contentType);
                uploadedUrls.Add(url);
            }

            if (!uploadedUrls.Any())
            {
                return BadRequest("No photos were uploaded.");
            }

            var success = await _photoHistoryService.SaveUploadedPhotos(request.SessionCode, uploadedUrls);

            if (!success)
            {
                return NotFound("SessionCode not found.");
            }

            return Ok(new { Message = "Photos uploaded and saved successfully.", UploadedUrls = uploadedUrls });
        }


        [AllowAnonymous]
        [HttpPost("upload-url")]
        public async Task<IActionResult> UploadUrlPhotos([FromBody] PhotoHistoryUrlRequestDTO request)
        {
            if (request.PhotoPaths == null || !request.PhotoPaths.Any() || string.IsNullOrEmpty(request.SessionCode))
            {
                return BadRequest("Invalid request data.");
            }

            var success = await _photoHistoryService.SaveUploadedPhotos(request.SessionCode, request.PhotoPaths);

            if (!success)
            {
                return NotFound("SessionCode not found.");
            }

            return Ok(new { Message = "Photos uploaded and saved successfully." });
        }

    }

}
