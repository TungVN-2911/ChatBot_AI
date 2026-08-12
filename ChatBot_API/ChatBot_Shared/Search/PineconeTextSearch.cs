using ChatBot_Shared.Clients;
using ChatBot_Shared.Models;
using Microsoft.SemanticKernel.Data;
using System.Text.Json;

namespace ChatBot_Shared.Search
{
    public class PineconeTextSearch : ITextSearch
    {
        private const double MinScoreThreshold = 0.7;

        private readonly PineconeVectorStore _pineconeVectorStore;
        private readonly OllamaEmbeddingClient _ollamaEmbeddingClient;

        public PineconeTextSearch(PineconeVectorStore pineconeVectorStore, OllamaEmbeddingClient ollamaEmbeddingClient)
        {
            _pineconeVectorStore = pineconeVectorStore;
            _ollamaEmbeddingClient = ollamaEmbeddingClient;
        }

        private async Task<List<PineconeMatch>> QueryRelevantMatchesAsync(string query, int topK, CancellationToken cancellationToken)
        {
            var embedding = await _ollamaEmbeddingClient.GetEmbeddingsAsync(query);
            var pineconeResponse = await _pineconeVectorStore.QueryAsync(embedding, topK);
            var parsedResponse = JsonSerializer.Deserialize<PineconeQueryResponse>(pineconeResponse);
            var matches = parsedResponse?.Matches ?? new List<PineconeMatch>();
            return matches.Where(m => m.Score >= MinScoreThreshold).ToList();
        }

        private static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                yield return item;
            }
            await Task.CompletedTask;
        }

        public async Task<KernelSearchResults<string>> SearchAsync(string query, TextSearchOptions? searchOptions = null, CancellationToken cancellationToken = default)
        {
            var matches = await QueryRelevantMatchesAsync(query, searchOptions?.Top ?? 5, cancellationToken);
            var texts = matches.Select(m => m.Metadata.GetValueOrDefault("Text", string.Empty)).ToList();
            return new KernelSearchResults<string>(ToAsyncEnumerable(texts), texts.Count);
        }

        public async Task<KernelSearchResults<TextSearchResult>> GetTextSearchResultsAsync(string query, TextSearchOptions? searchOptions = null, CancellationToken cancellationToken = default)
        {
            var matches = await QueryRelevantMatchesAsync(query, searchOptions?.Top ?? 5, cancellationToken);
            var results = matches.Select(m => new TextSearchResult(m.Metadata.GetValueOrDefault("Text", string.Empty))
            {
                Name = m.Metadata.GetValueOrDefault("Name", m.Id)
            }).ToList();
            return new KernelSearchResults<TextSearchResult>(ToAsyncEnumerable(results), results.Count);
        }

        public Task<KernelSearchResults<object>> GetSearchResultsAsync(string query, TextSearchOptions? searchOptions = null, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException($"{nameof(GetSearchResultsAsync)} is not used by this implementation.");
        }
    }
}
