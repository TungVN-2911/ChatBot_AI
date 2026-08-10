using ChatBot_Indexing.Models;
using ChatBot_Indexing.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot_Indexing.Clients
{
    public class PineconeVectorStore
    {
        private readonly HttpClient _httpClient;
        private readonly string _indexName;
        private string? _dataPlaneHost;
        public PineconeVectorStore(HttpClient httpClient, IOptions<AppOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.pinecone.io/");
            _httpClient.DefaultRequestHeaders.Add("Api-Key", options.Value.PineconeApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-Pinecone-API-Version", "2024-07");
            _indexName = options.Value.PineconeIndexName;
        }
        private async Task<string> GetDataPlaneHostAsync()
        {
            if (_dataPlaneHost != null)
            {
                return _dataPlaneHost;
            }
            var response = await _httpClient.GetAsync($"indexes/{_indexName}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PineconeIndexResponse>();
            _dataPlaneHost = $"https://{result!.Host}";
            return _dataPlaneHost;
        }
        public async Task UpsertAsync(List<EmbeddedChunk> embeddedChunks)
        {
            string host = await GetDataPlaneHostAsync();
            const int batchSize = 100;
            for (int i = 0; i<embeddedChunks.Count; i+=batchSize)
            {
                var batch = embeddedChunks.Skip(i).Take(batchSize).Select(ec => new
                {
                    id = ec.Chunk.Id,
                    values = ec.Vector,
                    metadata = ec.Chunk.Metadata
                });
                var response = await _httpClient.PostAsJsonAsync($"{host}/vectors/upsert", new { vectors = batch });
                response.EnsureSuccessStatusCode();
            }
        }
        public async Task<string> QueryAsync(float[] queryVector, int topK = 5)
        {
            string host = await GetDataPlaneHostAsync();
            var response = await _httpClient.PostAsJsonAsync($"{host}/query", new
            {
                vector = queryVector,
                topK,
                includeMetadata = true
            });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
