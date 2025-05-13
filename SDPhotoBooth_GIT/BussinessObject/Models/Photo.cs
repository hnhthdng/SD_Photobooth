using BussinessObject.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class Photo : BaseEntityNoAudit
    {
        [Required]
        public int PhotoHistoryId { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int PhotoStyleId { get; set; }

        [ForeignKey("PhotoHistoryId")]
        public virtual PhotoHistory PhotoHistory { get; set; }

        [ForeignKey("PhotoStyleId")]
        public virtual PhotoStyle PhotoStyle { get; set; }
    }
}
