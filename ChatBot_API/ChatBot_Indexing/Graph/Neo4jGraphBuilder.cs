using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatBot_Indexing.Models;
using Neo4j.Driver;

namespace ChatBot_Indexing.Graph
{
    public class Neo4jGraphBuilder
    {
        private readonly IDriver _iDriver;

        public Neo4jGraphBuilder(string uri, string username, string password)
        {
            _iDriver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
        }
        public async Task BuildTeamsAndPlayersGraphAsync(List<Team> teams)
        {
            using var session = _iDriver.AsyncSession();
            foreach (var team in teams)
            {
                await session.RunAsync(
                    "MERGE (t:Team {id: $id}) SET t.name = $name, t.venue = $venue, t.founded = $founded",
                    new { id = team.Id, name = team.Name, venue = team.Venue ?? "", founded = team.Founded ?? 0 }
                    );
                foreach (var player in team.Squad)
                {
                    await session.RunAsync(
                        "MERGE (p:Player {id: $id}) SET p.name = $name, p.nationality = $nat, p.position = $pos",
                        new { id = player.Id, name = player.Name, nat = player.Nationality ?? "", pos = player.Position ?? "" }
                        );
                    await session.RunAsync(
                        "MATCH (p:Player {id: $playerId}), (t:Team {id: $teamId}) " + "MERGE (p)-[:PLAYS_FOR]->(t)",
                        new { playerId = player.Id, teamId = team.Id }
                        );
                }
            }
        }
        public async Task BuildMatchesAsync(List<Match> matches)
        {
            using var session = _iDriver.AsyncSession();

            foreach (var match in matches)
            {
                // Tạo Match node
                await session.RunAsync(
                    "MERGE (m:Match {id: $id}) SET m.date = $date, m.stage = $stage, " +
                    "m.homeScore = $homeScore, m.awayScore = $awayScore",
                    new
                    {
                        id = match.Id,
                        date = match.UtcDate.ToString("yyyy-MM-dd"),
                        stage = match.Stage,
                        homeScore = match.Score?.FullTime?.Home,
                        awayScore = match.Score?.FullTime?.Away
                    });

                // Relationship: đội nhà -> trận
                await session.RunAsync(
                    "MATCH (t:Team {id: $teamId}), (m:Match {id: $matchId}) " +
                    "MERGE (t)-[:PLAYED_HOME]->(m)",
                    new { teamId = match.HomeTeam.Id, matchId = match.Id });

                // Relationship: đội khách -> trận
                await session.RunAsync(
                    "MATCH (t:Team {id: $teamId}), (m:Match {id: $matchId}) " +
                    "MERGE (t)-[:PLAYED_AWAY]->(m)",
                    new { teamId = match.AwayTeam.Id, matchId = match.Id });
            }
        }

        public async Task DisposeAsync() => await _iDriver.DisposeAsync();
    }
}
