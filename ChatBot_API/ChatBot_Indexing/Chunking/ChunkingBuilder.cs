using ChatBot_Indexing.Models;
using ChatBot_Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot_Indexing.Chunking
{
    public class ChunkingBuilder
    {
        public static List<Chunk> BuildMatchChunks(List<Match> matches)
        {
            List<Chunk> chunks = new List<Chunk>();
            foreach (Match match in matches)
            {
                string text = $"On {match.UtcDate:yyyy-MM-dd}, {match.HomeTeam.Name} played {match.AwayTeam.Name} in {match.Stage}. " +
                    $"Result: {match.HomeTeam.Name} {match.Score.FullTime.Home} - {match.Score.FullTime.Away} {match.AwayTeam.Name}.";
                chunks.Add(new Chunk
                {
                    Id = $"match-{match.Id}",
                    Text = text,
                    Metadata = new Dictionary<string, string>
                    {
                        { "Id", match.Id.ToString() },
                        { "UtcDate", match.UtcDate.ToString() },
                        { "Status", match.Status },
                        { "Matchday", match.Matchday?.ToString() ?? "" },
                        { "Stage", match.Stage },
                        { "HomeTeamId", match.HomeTeam?.Id.ToString() ?? "" },
                        { "HomeTeamName", match.HomeTeam?.Name ?? "" },
                        { "AwayTeamId", match.AwayTeam?.Id.ToString() ?? "" },
                        { "AwayTeamName", match.AwayTeam?.Name ?? "" },
                        { "ScoreFullTimeHome", match.Score?.FullTime?.Home.ToString() ?? "" },
                        { "ScoreFullTimeAway", match.Score?.FullTime?.Away.ToString() ?? "" },
                        { "Type", "match"},
                        { "Competition", "CL" },
                        { "Text", text }
                    }
                });
            }
            return chunks;
        }
        public static List<Chunk> BuildPlayerChunks(List<Team> teams)
        {
            List<Chunk> chunks = new List<Chunk>();
            foreach (Team team in teams)
            {
                foreach (Player player in team.Squad)
                {
                    string text = $"{player.Name} plays for {team.Name} as a {player.Position}, nationality {player.Nationality}.";
                    chunks.Add(new Chunk
                    {
                        Id = $"player-{player.Id}",
                        Text = text,
                        Metadata = new Dictionary<string, string>
                        {
                            { "Id", player.Id.ToString() },
                            { "Name", player.Name },
                            { "Position", player.Position ?? "" },
                            { "Nationality", player.Nationality ?? "" },
                            { "DateOfBirth", player.DateOfBirth?.ToString("yyyy-MM-dd") ?? "" },
                            { "TeamId", team.Id.ToString() },
                            { "TeamName", team.Name },
                            { "Type", "player"},
                            { "Competition", "CL" },
                            { "Text", text }
                        }
                    });
                }
            }
            return chunks;
        }
        public static List<Chunk> BuildSquadListChunks(List<Team> teams)
        {
            List<Chunk> chunks = new List<Chunk>();
            foreach (Team team in teams)
            {
                string text = $"{team.Name} squad includes: {string.Join(", ", team.Squad.Select(p => p.Name))}";
                chunks.Add(new Chunk
                {
                    Id = $"squad-{team.Id}",
                    Text = text,
                    Metadata = new Dictionary<string, string>
                    {
                        { "TeamId", team.Id.ToString() },
                        { "TeamName", team.Name },
                        { "SquadCount", team.Squad.Count.ToString() },
                        { "Type", "squad_list"},
                        { "Competition", "CL" },
                        { "Text", text }
                    }
                });
            }
            return chunks;
        }
        public static List<Chunk> BuildTeamInfoChunks(List<Team> teams)
        {
            List<Chunk> chunks = new List<Chunk>();
            foreach (Team team in teams)
            {
                string text = $"{team.Name} is based at {team.Venue}, founded in {team.Founded}";
                chunks.Add(new Chunk
                {
                    Id = $"team-{team.Id}",
                    Text = text,
                    Metadata = new Dictionary<string, string>
                    {
                        { "Id", team.Id.ToString() },
                        { "Name", team.Name },
                        { "Founded", team.Founded?.ToString() ?? "" },
                        { "Venue", team.Venue ?? "" },
                        { "Type", "team"},
                        { "Competition", "CL" },
                        { "Text", text }
                    }
                });
            }
            return chunks;
        }
    }
}
