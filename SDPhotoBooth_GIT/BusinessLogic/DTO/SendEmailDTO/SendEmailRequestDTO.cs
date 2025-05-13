using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.SendEmailDTO
{
    public class SendEmailRequestDto
    {
        public string toEmail { get; set; }
        public string photoUrl { get; set; }
    }
}
