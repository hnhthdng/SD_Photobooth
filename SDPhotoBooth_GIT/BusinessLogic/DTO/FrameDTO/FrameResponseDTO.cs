using BusinessLogic.DTO.CoordinateDTO;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.FrameDTO
{
    public class FrameResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string FrameUrl { get; set; }
        public string FrameStyleName { get; set; }
        public int SlotCount { get; set; }
        public bool? ForMobile { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }
        public ICollection<CordianteOnFrameResponseDTO> Coordinates { get; set; }
    }

    public class CordianteOnFrameResponseDTO
    {
        public int Id { get; set; }
        public int FrameId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
