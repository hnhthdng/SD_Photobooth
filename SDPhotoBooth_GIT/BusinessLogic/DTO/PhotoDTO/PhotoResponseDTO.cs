using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.PhotoDTO
{
    public class PhotoResponseDTO
    {
        public string Url { get; set; }

        public string PhotoStyleName { get; set; }
    }
}
