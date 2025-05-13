using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.GoogleDTO
{
    public class GoogleBillingRequest
    {
        public string TypeSessionName { get; set; }
        public long? OrderCode { get; set; }
        public int? DepositId { get; set; }
        public string PurchaseToken { get; set; }
    }
}
