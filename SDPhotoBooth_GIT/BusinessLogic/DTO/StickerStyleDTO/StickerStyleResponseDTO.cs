namespace BusinessLogic.DTO.StickerStyleDTO
{
    public class StickerStyleResponseDTO
    {
        public int Id { get; set; }
        public string StickerStyleName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public string CreatedById { get; set; }
        public string LastModifiedById { get; set; }

        public ICollection<StickerOnStickerStyleResponseDTO> Stickers { get; set; }
    }

    public class StickerOnStickerStyleResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StickerUrl { get; set; }
    }
}
