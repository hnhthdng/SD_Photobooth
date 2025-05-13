using AutoMapper;
using BusinessLogic.DTO.SessionCodeDTO;
using BusinessLogic.Service.IService;
using DataAccess.Extensions.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff, Manager")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService, IMapper mapper)
        {
            _sessionService = sessionService;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _sessionService.GetCount();
            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationParams pagination)
        {
            try
            {
                var sessions = await _sessionService.GetAllSessions(pagination);
                return Ok(sessions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("by-code/{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var session = await _sessionService.SearchSessionByCode(code);
            if (session == null)
            {
                return NotFound();
            }
            return Ok(session);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            var session = await _sessionService.GetSessionById(Id);
            if (session == null)
            {
                return NotFound();
            }
            return Ok(session);
        }

        [HttpPost("{orderCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(long orderCode)
        {
            var session = await _sessionService.CreateSession(orderCode);
            if (session == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create session.");
            }
            return Ok(session); 
        }


        [HttpPost("use-session")]
        [AllowAnonymous]
        public async Task<IActionResult> UseSession([FromBody] SessionRequestDTO request)
        {
            var session = await _sessionService.GetSessionByCode(request.Code);
            if (session == null)
            {
                return NotFound();
            }

            var result = await _sessionService.UseSession(request.Code, request.BoothId);

            return Ok(result); 
        }


    }
}