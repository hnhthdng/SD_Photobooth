using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.StickerDTO
{
    public class StickerRequestDTO
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        public string? StickerUrl { get; set; }

        public int? StickerStyleId { get; set; }
    }

    public class StickerWithFormFileRequestDTO
    {
        [MaxLength(50)]
        public string? Name { get; set; }
        public IFormFile? StickerFile { get; set; }
        public int? StickerStyleId { get; set; }
    }
}
