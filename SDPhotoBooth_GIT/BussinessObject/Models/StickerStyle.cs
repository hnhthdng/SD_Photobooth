using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class StickerStyle : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string StickerStyleName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public virtual ICollection<Sticker> Stickers { get; set; } = new List<Sticker>();
    }

}
