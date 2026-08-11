using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatBot_Shared.Clients;

namespace ChatBot_API.Controllers
{
    [Route("")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly OllamaEmbeddingClient _ollamaEmbeddingClient;
        private readonly PineconeVectorStore _pineconeVectorStore;
        public SearchController(OllamaEmbeddingClient ollamaEmbeddingClient, PineconeVectorStore pineconeVectorStore)
        {
            _ollamaEmbeddingClient = ollamaEmbeddingClient;
            _pineconeVectorStore = pineconeVectorStore;
        }
        [HttpPost("/api/search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest request)
        {
            var searchResults = await _ollamaEmbeddingClient.GetEmbeddingsAsync(request.Question);
            var results = await _pineconeVectorStore.QueryAsync(searchResults, request.TopK);
            return Content(results, "application/json");
        }
    }
    public class SearchRequest
    {
        public string Question { get; set; } = string.Empty;
        public int TopK { get; set; } = 5;
    }
}
