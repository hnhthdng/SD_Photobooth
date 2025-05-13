using BusinessLogic.DTO.MembershipCardDTO;
using BusinessLogic.DTO.WalletDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Enums;
using BussinessObject.Models;
using BussinessObject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Helpers;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWalletService _walletService;
        private readonly IMembershipCardService _membershipCardService;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IConfiguration configuration, IWalletService _walletService, IMembershipCardService membershipCardService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            this._walletService = _walletService;
            _membershipCardService = membershipCardService;
        }

        [HttpPost("google/callback")]
        public async Task<IActionResult> GoogleCallback([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                var payload = await GoogleTokenHelper.VerifyAccessTokenAsync(tokenRequest.Token, _configuration["GoogleOath2:WebClient"], _configuration["GoogleOath2:MobileClient"]);
                if (payload == null)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                User user = _userManager.FindByEmailAsync(payload.Email).Result;

                if (user == null)
                {
                    return Unauthorized(new { message = "User not found" });
                }
                else
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles == null || !roles.Any())
                    {
                        return Unauthorized(new { message = "User has no roles assigned" });
                    }

                    if (!Enum.TryParse<UserType>(roles[0], out var userRole) || (userRole != UserType.Admin && userRole != UserType.Manager && userRole != UserType.Staff))
                    {
                        return Unauthorized(new { message = "You don't have permission to access!" });
                    }
                    else
                    {
                        return Ok(new { message = "Token is valid", token = JwtHelper.GenerateJwtToken(user.Id, user.UserName, userRole, _configuration["Jwt:PrivateKey"]) });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Decryption failed", error = ex.Message });
            }
        }

        [HttpPost("google/sign-in")]
        public async Task<IActionResult> GoogleSignIn([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                var payload = await GoogleTokenHelper.VerifyAccessTokenAsync(tokenRequest.Token, _configuration["GoogleOath2:WebClient"], _configuration["GoogleOath2:MobileClient"]);
                if (payload == null)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                User user = _userManager.FindByEmailAsync(payload.Email).Result;

                if (user == null)
                {
                    var newUser = new User
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        FullName = payload.Name,
                        Avatar = payload.Picture
                    };

                    var createResult = await _userManager.CreateAsync(newUser);
                    if (!createResult.Succeeded)
                    {
                        return BadRequest(new { message = "Failed to create user", errors = createResult.Errors });
                    }

                    await _userManager.AddToRoleAsync(newUser, UserType.Customer.ToString());
                    await _walletService.CreateWallet(newUser.Id, new WalletRequestDTO());

                    await _membershipCardService.Create(new CreateMembershipCardRequestDTO()
                    {
                        CustomerId = newUser.Id,
                        LevelMemberShipId = 1,
                        Points = 0,
                        IsActive = true
                    });

                    return Ok(new { message = "Token is valid", token = JwtHelper.GenerateJwtToken(newUser.Id, newUser.UserName, UserType.Customer, _configuration["Jwt:PrivateKey"]) });
                }

                var roles = await _userManager.GetRolesAsync(user);
                var tf = Enum.TryParse<UserType>(roles[0], out var userRole);

                if (!tf || userRole != UserType.Customer)
                {
                    return Unauthorized(new { message = "You don't have permission to access! Please using another email" });
                }
                else
                {
                    return Ok(new { message = "Token is valid", token = JwtHelper.GenerateJwtToken(user.Id, user.UserName, userRole, _configuration["Jwt:PrivateKey"]) });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Decryption failed", error = ex.Message });
            }
        }
    }
}
