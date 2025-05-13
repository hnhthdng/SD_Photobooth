using BussinessObject.Enums;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.PhotoStyleDTO
{
    public class PhotoStyleResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string Prompt { get; set; }
        public string NegativePrompt { get; set; }
        public string? Controlnets { get; set; }
        public int? NumImagesPerGen { get; set; } 
        public string? BackgroundColor { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public double? IPAdapterScale { get; set; }
        public InpaintMode? Mode { get; set; }
        public int? NumInferenceSteps { get; set; }
        public double? GuidanceScale { get; set; }
        public double? Strength { get; set; }
        public bool? BackgroundRemover { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }

    }
}
