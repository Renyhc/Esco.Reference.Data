using Newtonsoft.Json;
using System.Collections.Generic;

namespace ESCO.Reference.Data.Model
{     
    public class Horizons
    {
        [JsonProperty("value")]
        public List<Horizon> value { get; set; }
    }

    public class Horizon
    {
        [JsonProperty("HorizonId")]
        public string HorizonId { get; set; }

        [JsonProperty("HorizonName")]
        public string HorizonName { get; set; }
    }
}
