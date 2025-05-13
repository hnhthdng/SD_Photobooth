using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;


namespace BussinessObject.Models
{
    public class Booth : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new int Id { get; set; } = new Random().Next(100000000, 999999999);

        [Required]
        public int LocationId { get; set; }

        [Required]
        [StringLength(100)]
        public string? BoothName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        public virtual ICollection<PhotoHistory> PhotoHistories { get; set; } = new List<PhotoHistory>();
    }
}