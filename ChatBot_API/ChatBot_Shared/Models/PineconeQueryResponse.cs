using System.Text.Json.Serialization;

namespace ChatBot_Shared.Models
{
    public class PineconeQueryResponse
    {
        [JsonPropertyName("matches")]
        public List<PineconeMatch> Matches { get; set; } = new();
    }

    public class PineconeMatch
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("metadata")]
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}