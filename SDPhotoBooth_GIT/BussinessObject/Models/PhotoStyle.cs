using BussinessObject.Enums;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class PhotoStyle : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string Prompt { get; set; }

        public string NegativePrompt { get; set; }

        public string? Controlnets { get; set; }

        public int? NumImagesPerGen { get; set; } = 6;

        public string? BackgroundColor { get; set; }

        public int? Height { get; set; }

        public int? Width { get; set; }

        public double? IPAdapterScale { get; set; }

        public InpaintMode? Mode { get; set; } = InpaintMode.None;
        public int? NumInferenceSteps { get; set; }
        public double? GuidanceScale { get; set; }
        public double? Strength { get; set; }
        public bool? BackgroundRemover { get; set; }

        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}