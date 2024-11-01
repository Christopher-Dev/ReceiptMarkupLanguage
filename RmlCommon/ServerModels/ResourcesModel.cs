using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RmlCommon.ServerModels
{
    public class ResourcesModel
    {
        [JsonPropertyName("resourceContent")]
        public string ResourceContent { get; set; }

    }
}
