using System.Text.Json.Serialization;

namespace ChatBot_Indexing.Models
{
    public class TeamsResponse
    {
        [JsonPropertyName("teams")]
        public List<Team> Teams { get; set; } = new();
    }

    public class Team
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("venue")]
        public string? Venue { get; set; }

        [JsonPropertyName("founded")]
        public int? Founded { get; set; }

        [JsonPropertyName("squad")]
        public List<Player> Squad { get; set; } = new();
    }

    public class Player
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("position")]
        public string? Position { get; set; }

        [JsonPropertyName("nationality")]
        public string? Nationality { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }
    }
}
