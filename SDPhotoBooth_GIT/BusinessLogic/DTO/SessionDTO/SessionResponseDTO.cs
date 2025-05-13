namespace BusinessLogic.DTO.SessionCodeDTO
{
    public class SessionResponseDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? Expired { get; set; }
        public bool IsActive { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsDeleted { get; set; }
        public int AbleTakenNumber { get; set; }
    }
}