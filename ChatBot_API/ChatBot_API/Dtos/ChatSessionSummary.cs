namespace ChatBot_API.Dtos
{
    public class ChatSessionSummary
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PreviewText { get; set; } = string.Empty;
    }
}
