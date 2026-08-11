namespace ChatBot_Shared.Options
{
    public class AppOptions
    {
        public const string SectionName = "App";

        public string PineconeApiKey { get; set; } = string.Empty;
        public string PineconeIndexName { get; set; } = "epl-chatbot";
        public string OllamaBaseUrl { get; set; } = "http://localhost:11434";
        public string OllamaEmbeddingModel { get; set; } = "nomic-embed-text";
    }
}
