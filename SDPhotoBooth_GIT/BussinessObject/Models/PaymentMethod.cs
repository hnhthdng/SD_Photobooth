using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class PaymentMethod : BaseEntity
    {
        [Required]
        [StringLength(100, ErrorMessage = "MethodName cannot exceed 100 characters.")]
        public string MethodName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public bool? ForMobile { get; set; } = false;

        public bool? IsOnline { get; set; } = false;

        public virtual ICollection<Payment> Payments { get; set; }
    }
}