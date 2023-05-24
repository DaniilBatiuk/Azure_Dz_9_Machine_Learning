using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Dz_9_Machine_Learning
{
    public class BikeShareResult
    {
        [JsonProperty("Results")]
        public IList<double> Results { get; set; }

    }
}
