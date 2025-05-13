

using Microsoft.AspNetCore.Http;

namespace BusinessLogic.DTO.ImageProcessingDTO
{
    public class ImageProcessingDTO
    {
        public IFormFile image { get; set; }

        public int photoStyleId { get; set; }

        public string connectionId { get; set; }

        public string sessionCode { get; set; }
    }

}
