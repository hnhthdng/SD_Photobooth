using BusinessLogic.DTO.LocationDTO;

namespace BusinessLogic.DTO.BoothDTO
{
    public class BoothResponseDTO
    {
        public int Id { get; set; }
        public string? BoothName { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; } 
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }
        public LocationResponseDTO? Location { get; set; }
    }
}
