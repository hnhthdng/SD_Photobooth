using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DTO.DepositProductDTO
{
    public class DepositProductCreateRequestDTO
    {
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? AmountAdd { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
