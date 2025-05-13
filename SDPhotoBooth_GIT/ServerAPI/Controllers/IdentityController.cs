using AutoMapper;
using Azure.Core;
using BusinessLogic.DTO.IdentityDTO;
using BusinessLogic.DTO.UserDTO;
using BusinessLogic.Service.IService;
using BussinessObject.Enums;
using BussinessObject.Models;
using BussinessObject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using ServerAPI.Helpers;
using System.Text;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly IEmailService _emailService;
        private readonly FirebaseService _firebaseService;

        public IdentityController(UserManager<User> userManager, SignInManager<User> signInManager,
            IConfiguration configuration, IMapper mapper, IDistributedCache cache, IEmailService emailService, FirebaseService firebaseService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
            _cache = cache;
            _emailService = emailService;
            _firebaseService = firebaseService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (user.IsDeleted)
                {
                    return Unauthorized(new { message = "Your account has been deleted." });
                }

                if (user.IsBanned)
                {
                    return Unauthorized(new { message = "Your account has been banned." });
                }

                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                if (signInResult.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles == null || !roles.Any())
                    {
                        return Unauthorized(new { message = "User has no roles assigned" });
                    }

                    if (!Enum.TryParse<UserType>(roles[0], out var userRole) || !Enum.IsDefined(typeof(UserType), userRole) || (userRole != UserType.Admin && userRole != UserType.Manager && userRole != UserType.Staff && userRole != UserType.Customer))
                    {
                        return Unauthorized(new { message = "You don't have permission to access!" });
                    }

                    return Ok(new { message = "Token is valid", token = JwtHelper.GenerateJwtToken(user.Id, user.UserName, userRole, _configuration["Jwt:PrivateKey"]) });
                }
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token không hợp lệ" });
            }

            var expirationTime = DateTimeOffset.UtcNow.AddHours(5); 
            await _cache.SetStringAsync($"blacklist:{token}", "expired", new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expirationTime
            });

            await _signInManager.SignOutAsync();

            return Ok(new { message = "Đăng xuất thành công" });
        }



        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var userDTO = _mapper.Map<UserResponseDTO>(user);
            userDTO.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            return Ok(userDTO);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPatch("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                var email = await _userManager.FindByEmailAsync(model.Email);
                if (email != null && email.Id != user.Id)
                {
                    return BadRequest(new { message = "Email already exists" });
                }
                user.Email = model.Email;

            }
            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                var phoneNumber = await _userManager.FindByEmailAsync(model.PhoneNumber);
                if (phoneNumber != null && phoneNumber.Id != user.Id)
                {
                    return BadRequest(new { message = "Phone number already exists" });
                }
                user.PhoneNumber = model.PhoneNumber;
            }
            if (model.Gender.HasValue) user.Gender = model.Gender.Value;
            if (model.BirthDate.HasValue) user.BirthDate = model.BirthDate.Value;
            if(!string.IsNullOrEmpty(model.FullName)) user.FullName = model.FullName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(user);
        }

        [HttpPut("update-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAvatar([FromForm] UpdateAvatarRequest request)
        {
            var avatar = request.Avatar;
            if (avatar == null || avatar.Length == 0)
                return BadRequest("Avatar file is required.");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var uploadedUrl = await _firebaseService.UploadImageAsync(avatar.OpenReadStream(), avatar.FileName, avatar.ContentType);

            user.Avatar = uploadedUrl;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Avatar updated successfully", avatarUrl = uploadedUrl });
        }


        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email is required.");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("User with this email does not exist.");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var feDomain = _configuration["ClientUrl:Production"];
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetLink = $"{feDomain}/reset-password?email={request.Email}&token={encodedToken}";
            await _emailService.SendResetPasswordEmail(user.Email, resetLink);
            return Ok(new { message = "Reset password link has been sent to your email" });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("All fields are required.");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("Invalid email.");
            }

            byte[] tokenBytes;
            try
            {
                tokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            }
            catch
            {
                return BadRequest("Invalid token format.");
            }

            var origToken = Encoding.UTF8.GetString(tokenBytes);

            var isValidToken = await _userManager.VerifyUserTokenAsync(
               user,
               _userManager.Options.Tokens.PasswordResetTokenProvider,
               "ResetPassword",
               origToken);

            if (!isValidToken)
            {
                return BadRequest("Invalid or expired token.");
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, origToken, request.NewPassword);
            if (!resetPassResult.Succeeded)
            {
                return BadRequest(resetPassResult.Errors.Select(e => e.Description));
            }

            return Ok("Password has been reset successfully.");
        }


        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return Unauthorized();
        }
    }
}