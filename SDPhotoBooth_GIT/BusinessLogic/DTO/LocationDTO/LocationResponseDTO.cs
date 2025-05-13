namespace BusinessLogic.DTO.LocationDTO
{
    public class LocationResponseDTO
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime ?LastModified { get; set; }
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }
    }
}
