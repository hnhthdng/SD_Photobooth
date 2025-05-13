namespace BusinessLogic.DTO.TypeSessionDTO
{
    public class TypeSessionResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public decimal Price { get; set; }
        public bool ForMobile { get; set; } 
        public int? AbleTakenNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }
    }
}
