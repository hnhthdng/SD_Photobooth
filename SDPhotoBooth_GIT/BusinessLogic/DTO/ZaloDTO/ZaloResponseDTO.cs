using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.ZaloDTO
{
    public class ZaloResponseDTO
    {
        public string orderCode { get; set; }
        public string[] photoUrls { get; set; }
        public string customerName { get; set; }
    }
}
