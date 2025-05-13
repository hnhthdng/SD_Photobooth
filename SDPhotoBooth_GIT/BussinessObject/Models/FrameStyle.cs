using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class FrameStyle : BaseEntity
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<Frame> Frames { get; set; } = new List<Frame>();
    }
}
