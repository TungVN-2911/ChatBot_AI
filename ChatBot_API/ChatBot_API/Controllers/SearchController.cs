using ChatBot_API.Dtos;
using ChatBot_Shared.Clients;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot_API.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly OllamaEmbeddingClient _ollamaEmbeddingClient;
        private readonly PineconeVectorStore _pineconeVectorStore;

        public SearchController(OllamaEmbeddingClient ollamaEmbeddingClient, PineconeVectorStore pineconeVectorStore)
        {
            _ollamaEmbeddingClient = ollamaEmbeddingClient;
            _pineconeVectorStore = pineconeVectorStore;
        }

        [HttpPost]
        public async Task<IActionResult> Search([FromBody] SearchRequest request)
        {
            var queryVector = await _ollamaEmbeddingClient.GetEmbeddingsAsync(request.Question);
            var results = await _pineconeVectorStore.QueryAsync(queryVector, request.TopK);
            return Content(results, "application/json");
        }
    }
}
