using ChatBot_Shared.Models;
using ChatBot_Shared.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace ChatBot_Shared.Clients
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
