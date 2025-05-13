using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class Sticker : BaseEntity
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        public string StickerUrl { get; set; }

        public int? StickerStyleId { get; set; }

        [ForeignKey("StickerStyleId")]
        public virtual StickerStyle StickerStyle { get; set; }
    }
}