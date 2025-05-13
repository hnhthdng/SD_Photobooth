using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.GoogleDTO
{
    public class GooglePurchaseResponse
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("packageName")]
        public string PackageName { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("purchaseToken")]
        public string PurchaseToken { get; set; }

        [JsonProperty("purchaseTimeMillis")]
        public long PurchaseTimeMillis { get; set; }

        [JsonProperty("purchaseState")]
        public int PurchaseState { get; set; } // 0 = Purchased, 1 = Canceled, 2 = Pending

        [JsonProperty("acknowledged")]
        public bool Acknowledged { get; set; }
    }

}
