using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatBot_Indexing.Models
{
    public class OllamaEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        public List<float> Embedding { get; set; } = new();
    }
}
