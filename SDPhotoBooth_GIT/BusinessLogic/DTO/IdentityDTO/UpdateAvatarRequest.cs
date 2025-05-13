using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.IdentityDTO
{
    public class UpdateAvatarRequest
    {
        [Required]
        public IFormFile Avatar { get; set; }
    }
}
