using System.Text.Json.Serialization;

namespace ChatBot_Shared.Models
{
    public class OllamaEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        public List<float> Embedding { get; set; } = new();
    }
}
