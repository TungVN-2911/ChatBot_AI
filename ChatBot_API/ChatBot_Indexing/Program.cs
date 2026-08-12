using ChatBot_Indexing.Chunking;
using ChatBot_Indexing.Clients;
using ChatBot_Indexing.Graph;
using ChatBot_Indexing.Models;
using ChatBot_Indexing.Options;
using ChatBot_Shared.Clients;
using ChatBot_Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using SharedAppOptions = ChatBot_Shared.Options.AppOptions;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<AppOptions>(
    builder.Configuration.GetSection(AppOptions.SectionName));
builder.Services.Configure<SharedAppOptions>(
    builder.Configuration.GetSection(SharedAppOptions.SectionName));

builder.Services.AddHttpClient<FootballDataClient>();
builder.Services.AddHttpClient<OllamaEmbeddingClient>();
builder.Services.AddHttpClient<PineconeVectorStore>();

using var host = builder.Build();

var footballDataClient = host.Services.GetRequiredService<FootballDataClient>();

string matchesJson = await footballDataClient.GetChampionsLeagueMatchesAsync();
string teamsJson = await footballDataClient.GetChampionsLeagueTeamsAsync();

var matchesData = JsonSerializer.Deserialize<MatchesResponse>(matchesJson);
var teamsData = JsonSerializer.Deserialize<TeamsResponse>(teamsJson);

//var chunks = new List<Chunk>();
//chunks.AddRange(ChunkingBuilder.BuildMatchChunks(matchesData.Matches));
//chunks.AddRange(ChunkingBuilder.BuildTeamInfoChunks(teamsData.Teams));
//chunks.AddRange(ChunkingBuilder.BuildPlayerChunks(teamsData.Teams));
//chunks.AddRange(ChunkingBuilder.BuildSquadListChunks(teamsData.Teams));

//var ollamaEmbeddingClient = host.Services.GetRequiredService<OllamaEmbeddingClient>();

//var embeddedChunks = new List<EmbeddedChunk>();
//foreach(var chunk in chunks)
//{
//    var vector = await ollamaEmbeddingClient.GetEmbeddingsAsync(chunk.Text);
//    embeddedChunks.Add(new EmbeddedChunk { Chunk = chunk, Vector = vector });
//}

//var pineconeStore = host.Services.GetRequiredService<PineconeVectorStore>();
//await pineconeStore.UpsertAsync(embeddedChunks);

//var testVector = await ollamaEmbeddingClient.GetEmbeddingsAsync("Bukayo Saka plays for Arsenal, doesn't he?");
//string queryResult = await pineconeStore.QueryAsync(testVector);
//Console.WriteLine(queryResult);

var sharedAppOptions = host.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<SharedAppOptions>>().Value;
var graphBuilder = new Neo4jGraphBuilder(sharedAppOptions.Neo4jUri, sharedAppOptions.Neo4jUsername, sharedAppOptions.Neo4jPassword);
await graphBuilder.BuildTeamsAndPlayersGraphAsync(teamsData.Teams);
await graphBuilder.BuildMatchesAsync(matchesData.Matches);
await graphBuilder.DisposeAsync();