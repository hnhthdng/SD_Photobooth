using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.CoordinateDTO
{
    public class CoordinateRequestDTO
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

    }

    public class UpdateCoordinateDTO
    {
        public int? X { get; set; }
        public int? Y { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
