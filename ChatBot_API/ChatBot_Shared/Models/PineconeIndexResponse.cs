using System.Text.Json.Serialization;

namespace ChatBot_Shared.Models
{
    public class PineconeIndexResponse
    {
        [JsonPropertyName("host")]
        public string Host { get; set; } = string.Empty;
    }
}
