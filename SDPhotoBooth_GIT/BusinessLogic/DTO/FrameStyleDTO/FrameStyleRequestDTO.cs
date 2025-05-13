using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.FrameDTO
{
    public class FrameStyleRequestDTO : BaseFrameStyleDTO
    {
        public string? ImageUrl { get; set; }
    }
    public class FrameStyleWithFormFileDTO : BaseFrameStyleDTO
    {
        public IFormFile? Image { get; set; }
    }
    public class BaseFrameStyleDTO
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
