using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class MonthSellsResponse
    {
        public List<string> Categories { get; set; }
        public ChartSerie Serie { get; set; }
    }

    public class ChartSerie
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("data")]
        public List<object> Data { get; set; }
    }
}
