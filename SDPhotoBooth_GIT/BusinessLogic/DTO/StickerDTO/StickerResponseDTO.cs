namespace BusinessLogic.DTO.StickerDTO
{
    public class StickerResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StickerUrl { get; set; }
        public string? StickerStyleName { get; set; }
        public DateTime? CreatedAt { get; set; } 
        public DateTime? LastModified { get; set; } 
        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }
    }
}
