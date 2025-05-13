using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.StickerStyleDTO
{
    public class StickerStyleRequestDTO
    {
        [MaxLength(50)]
        public string? StickerStyleName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
