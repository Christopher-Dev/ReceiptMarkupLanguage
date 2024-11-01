using RmlCommon.ServerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RmlCommon
{
    public class RenderRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonPropertyName("oneBitPng")]
        public bool OneBitPng { get; set; } = true;

        [JsonPropertyName("mimeType")]
        public string MimeType { get; set; } = string.Empty;

        [JsonPropertyName("bodyContents")]
        public string BodyContents { get; set; } = string.Empty;
    }
}
