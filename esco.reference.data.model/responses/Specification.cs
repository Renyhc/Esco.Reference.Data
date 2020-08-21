using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace esco.reference.data.Model
{
    public  class Specification
    {
        [JsonProperty("instrumentTypes")]
        public dynamic instrumentTypes { get; set; }

        [JsonProperty("fieldTypes")]
        public dynamic fieldTypes { get; set; }

        [JsonProperty("fieldTypesByInstrumentTypes")]
        public dynamic fieldTypesByInstrumentTypes { get; set; }
    }
}
