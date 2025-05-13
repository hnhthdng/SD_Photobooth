using BusinessLogic.DTO.LocationDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("count")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _locationService.GetCount();
            return Ok(count);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> GetLocations([FromQuery] PaginationParams pagination)
        {
            try
            {
                var locations = await _locationService.GetAllLocations(pagination);
                return Ok(locations);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _locationService.GetLocation(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpGet("by-location/{locationName}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetByName(string locationName)
        {
            var location = await _locationService.GetLocation(locationName);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }


        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Post([FromBody] LocationRequestDTO locationRequestDTO)
        {
            if (locationRequestDTO == null)
            {
                return BadRequest("Invalid data provided.");
            }
            var location = await _locationService.CreateLocation(locationRequestDTO);
            return Ok(location);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Put(int id, [FromBody] LocationRequestDTO locationRequestDTO)
        {
            if (id <= 0 || locationRequestDTO == null)
            {
                return BadRequest("Invalid data provided.");
            }
            var location = await _locationService.GetLocation(id);
            if (location == null)
            {
                return NotFound();
            }
            var updatedLocation = await _locationService.UpdateLocation(id, locationRequestDTO);
            return Ok(updatedLocation);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid data provided.");
            }
            var location = await _locationService.GetLocation(id);
            if (location == null)
            {
                return NotFound();
            }
            var deletedLocationId = await _locationService.DeleteLocation(id);
            return Ok(deletedLocationId);
        }
    } 
}
