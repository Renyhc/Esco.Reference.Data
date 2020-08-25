using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESCO.Reference.Data.Model
{     
    public class Issuers
    {
        [JsonProperty("value")]
        public List<IssuerValue> value { get; set; }
    }

    public class IssuerValue
    {
        [JsonProperty("Issuer")]
        public string Issuer { get; set; }
    }
}
