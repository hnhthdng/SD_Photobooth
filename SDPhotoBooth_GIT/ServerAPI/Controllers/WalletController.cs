using BusinessLogic.DTO.WalletDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("get-wallet")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetWallet()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var wallet = await _walletService.GetWalletByCustomerId(userId);
            if (wallet == null)
            {
                wallet = await _walletService.CreateWallet(userId, new WalletRequestDTO());
            }
            return Ok(wallet);
        }

    }
}
