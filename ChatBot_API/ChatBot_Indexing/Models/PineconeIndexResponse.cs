using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatBot_Indexing.Models
{
    public class PineconeIndexResponse
    {
        [JsonPropertyName("host")]
        public string Host { get; set; } = string.Empty;
    }
}
