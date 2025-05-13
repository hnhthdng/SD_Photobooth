using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class TypeSession : BaseEntity
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Duration must be a non-negative value.")]
        public int Duration { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "AbleTakenNumber must be a non-negative value.")]
        public int AbleTakenNumber { get; set; }

        public bool? ForMobile { get; set; } = false;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<TypeSessionProduct> TypeSessionProducts { get; set; } = new List<TypeSessionProduct>();
    }
}