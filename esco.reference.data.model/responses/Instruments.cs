using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace esco.reference.data.Model
{
    public class Instruments : List<Instrument> { }    
    public class InstrumentsReport : List<InstrumentReport> { }
    public  class Instrument
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("schemaId")]
        public string schemaId { get; set; }

        [JsonProperty("active")]
        public bool active { get; set; }

        [JsonProperty("source")]
        public int source { get; set; }

        [JsonProperty("type")]
        public int type { get; set; }

        [JsonProperty("control")]
        public int control { get; set; }

        [JsonProperty("properties")]
        public List<Fields> properties { get; set; }    
    }

    public class InstrumentReport
    {
        [JsonProperty("id")]
        public string id { get; set; }     

        [JsonProperty("source")]
        public string source { get; set; }

        [JsonProperty("properties")]
        public dynamic properties { get; set; }
    }

    public class Fields
    {
        [JsonProperty("fieldId")]
        public string fieldId { get; set; }

        [JsonProperty("value")]
        public string value { get; set; }

        [JsonProperty("sourceValue")]
        public string sourceValue { get; set; }       

        [JsonProperty("origin")]
        public int origin { get; set; }

        [JsonProperty("control")]
        public int control { get; set; }
    }
}
