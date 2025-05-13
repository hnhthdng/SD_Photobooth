using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTO.PhotoDTO;

namespace BusinessLogic.DTO.PhotoHistoryDTO
{
    public class PhotoHistoryResponseDTO
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public PhotoResponseDTO[] Photos { get; set; }
        public decimal Amount { get; set; }
    }
}
