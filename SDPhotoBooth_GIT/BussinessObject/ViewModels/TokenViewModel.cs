using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.ViewModels
{
    public class TokenRequest
    {
        public string Token { get; set; }
    }

    public class TokenInfo
    {
        [JsonProperty("iss")]
        public string Iss { get; set; }

        [JsonProperty("aud")]
        public string Aud { get; set; }

        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("exp")]
        public long Exp { get; set; }

        [JsonProperty("access_type")]
        public string AccessType { get; set; }
    }
}
