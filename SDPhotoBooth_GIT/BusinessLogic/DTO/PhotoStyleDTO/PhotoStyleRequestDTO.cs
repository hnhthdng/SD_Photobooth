using BussinessObject.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.PhotoStyleDTO
{
    public class PhotoStyleRequestDTO : BasePhotoStyleDTO
    {
        public string? ImageUrl { get; set; }
    }

    public class PhotostyleWithFormFileRequestDTO : BasePhotoStyleDTO
    {
        public IFormFile? ImageUrl { get; set; }
    }

    public class BasePhotoStyleDTO 
    {
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public string? Prompt { get; set; }

        public string? NegativePrompt { get; set; }

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
    }
}
