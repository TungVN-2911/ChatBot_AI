namespace ChatBot_API.Models
{
    public enum ChatRole
    {
        User,
        Assistant
    }

    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid ChatSessionId { get; set; }
        public ChatSession? ChatSession { get; set; }

        public ChatRole Role { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
