namespace ChatBot_Shared.Models
{
    public class Chunk
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
    public class EmbeddedChunk
    {
        public Chunk Chunk { get; set; }
        public float[] Vector { get; set; }
    }
}
