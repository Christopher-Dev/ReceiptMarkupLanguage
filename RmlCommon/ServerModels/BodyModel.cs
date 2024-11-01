using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RmlCommon.ServerModels
{
    public class BodyModel
    {
        [JsonPropertyName("bodyContent")]
        public string BodyContent { get; set; }
    }
}
