using Microsoft.SemanticKernel;
using Neo4j.Driver;
using System.ComponentModel;

namespace ChatBot_API.Services
{
    public class GraphQueryPlugin
    {
        private readonly IDriver _driver;
        public GraphQueryPlugin(IDriver driver)
        {
            _driver = driver;
        }
        [KernelFunction("get_GermanyPlayers_of_team")]
        [Description("Get players who are German and play for this team in Champion League.")]
        public async Task<string> GetGermanPlayersOfTeamAsync(string teamName)
        {
            using var session = _driver.AsyncSession();
            var result = await session.RunAsync(
                "MATCH (t:Team {name: $teamName})<-[:PLAYS_FOR]-(p:Player {nationality: 'Germany'}) RETURN DISTINCT p.name AS player ORDER BY player",
                new { teamName }
                );
            var records = await result.ToListAsync();
            if (records.Count == 0)
            {
                return $"No German players found for team '{teamName}'.";
            }
            var lines = records.Select(r => $"{r["player"]} in {teamName} is German.");
            return string.Join("\n", lines);
        }
    }
}
