using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastModified { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public string? CreatedById { get; set; }
        public string? LastModifiedById { get; set; }


        [ForeignKey("CreatedById")]
        public virtual User CreatedBy { get; set; }

        [ForeignKey("LastModifiedById")]
        public virtual User LastModifiedBy { get; set; }

        public void UpdateTimestamp()
        {
            LastModified = DateTime.Now;
        }
    }
}