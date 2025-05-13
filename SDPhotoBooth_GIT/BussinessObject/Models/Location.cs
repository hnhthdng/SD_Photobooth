using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BussinessObject.Models
{
    public class Location : BaseEntity
    {
        [Required]
        [StringLength(100, ErrorMessage = "LocationName cannot exceed 100 characters.")]
        public string LocationName { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; }

        public virtual ICollection<PhotoHistory> PhotoHistories { get; set; } = new List<PhotoHistory>();

        public virtual ICollection<Booth> Booths { get; set; } = new List<Booth>();

        public virtual ICollection<User> Staffs { get; set; } = new List<User>();
    }
}