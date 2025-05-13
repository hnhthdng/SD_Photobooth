using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class TypeSessionProduct : BaseEntity
    {
        public string? Name { get; set; }

        [Required]
        public string ProductId { get; set; } 

        public int? CouponId { get; set; }
        [ForeignKey("CouponId")]
        public virtual Coupon Coupon { get; set; }

        public int? LevelMembershipId { get; set; }
        [ForeignKey("LevelMembershipId")]
        public virtual LevelMembership LevelMembership { get; set; }

        public int TypeSessionId { get; set; }
        [ForeignKey("TypeSessionId")]
        public virtual TypeSession TypeSession { get; set; }
    }
}
