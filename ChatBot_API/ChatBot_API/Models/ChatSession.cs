namespace ChatBot_API.Models
{
    public class ChatSession
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
