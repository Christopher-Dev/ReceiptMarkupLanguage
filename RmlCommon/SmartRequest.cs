using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RmlCommon
{
    public class SmartRequest<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("createdOn")]
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    }
}
