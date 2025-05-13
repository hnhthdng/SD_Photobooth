namespace BusinessLogic.DTO.TypeSessionDTO
{
    public class TypeSessionRequestDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public decimal? Price { get; set; }
        public int? AbleTakenNumber { get; set; }
        public bool? ForMobile { get; set; } 
    }
}
