using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public class MembershipCard : BaseEntity
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public int LevelMemberShipId { get; set; }

        public int Points { get; set; } = 0;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("CustomerId")]
        public virtual User Customer { get; set; }

        [ForeignKey("LevelMemberShipId")]
        public virtual LevelMembership LevelMemberShip { get; set; }
    }
}