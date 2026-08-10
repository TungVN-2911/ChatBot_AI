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
    public class OllamaEmbeddingClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;
        public OllamaEmbeddingClient(HttpClient httpClient, IOptions<AppOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.OllamaBaseUrl);
            _model = options.Value.OllamaEmbeddingModel;
        }
        public async Task<float[]> GetEmbeddingsAsync(string input)
        {
            var response = await _httpClient.PostAsJsonAsync("api/embeddings", new { model = _model, prompt = input });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>();
            return result is null ? Array.Empty<float>() : result.Embedding.ToArray();
        }
    }
}
