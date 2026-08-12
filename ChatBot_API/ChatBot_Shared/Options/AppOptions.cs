namespace ChatBot_Shared.Options
{
    public class AppOptions
    {
        public const string SectionName = "App";

        public string PineconeApiKey { get; set; } = string.Empty;
        public string PineconeIndexName { get; set; } = "epl-chatbot";
        public string OllamaBaseUrl { get; set; } = "http://localhost:11434";
        public string OllamaEmbeddingModel { get; set; } = "nomic-embed-text";
        public string GeminiApiKey { get; set; } = string.Empty;
        public string GeminiModelId { get; set; } = "gemini-flash-latest";
        public string Neo4jUri { get; set; } = "neo4j://localhost:7687";
        public string Neo4jUsername { get; set; } = "neo4j";
        public string Neo4jPassword { get; set; } = string.Empty;
    }
}
