using System.Text.Json.Serialization;

namespace ChatBot_Indexing.Models
{
    public class MatchesResponse
    {
        [JsonPropertyName("matches")]
        public List<Match> Matches { get; set; } = new();
    }

    public class Match
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("utcDate")]
        public DateTime UtcDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("matchday")]
        public int? Matchday { get; set; }

        [JsonPropertyName("stage")]
        public string Stage { get; set; } = string.Empty;

        [JsonPropertyName("homeTeam")]
        public TeamRef HomeTeam { get; set; } = new();

        [JsonPropertyName("awayTeam")]
        public TeamRef AwayTeam { get; set; } = new();

        [JsonPropertyName("score")]
        public Score Score { get; set; } = new();
    }

    public class TeamRef
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class Score
    {
        [JsonPropertyName("winner")]
        public string? Winner { get; set; }

        [JsonPropertyName("fullTime")]
        public FullTimeScore FullTime { get; set; } = new();
    }

    public class FullTimeScore
    {
        [JsonPropertyName("home")]
        public int? Home { get; set; }

        [JsonPropertyName("away")]
        public int? Away { get; set; }
    }
}
