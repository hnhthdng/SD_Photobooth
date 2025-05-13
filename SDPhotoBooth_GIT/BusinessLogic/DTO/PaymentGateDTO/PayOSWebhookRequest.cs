using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTO.PaymentGateDTO
{
    public class PayOSWebhookRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public PayOSWebhookData Data { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }

    public class PayOSWebhookData
    {
        [JsonProperty("orderCode")]
        public int OrderCode { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("transactionDateTime")]
        public string TransactionDateTime { get; set; }

        [JsonProperty("paymentLinkId")]
        public string PaymentLinkId { get; set; }
    }

}
