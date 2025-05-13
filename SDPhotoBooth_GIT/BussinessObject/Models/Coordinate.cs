using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class Coordinate : BaseEntityNoAudit
    {
        [Required]
        public int FrameId { get; set; }

        [Required]
        public int X { get; set; }

        [Required]
        public int Y { get; set; }

        [Required]
        public int Width { get; set; }

        [Required]
        public int Height { get; set; }

        [ForeignKey("FrameId")]
        public virtual Frame Frame { get; set; } = null!;
    }
}