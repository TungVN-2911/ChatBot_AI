using ChatBot_API.Data;
using ChatBot_API.Dtos;
using ChatBot_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot_API.Services
{
    public class ChatHistoryService
    {
        private readonly AppDbContext _db;

        public ChatHistoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ChatSession> CreateSessionAsync()
        {
            var session = new ChatSession
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            _db.ChatSessions.Add(session);
            await _db.SaveChangesAsync();
            return session;
        }

        public async Task<List<ChatSessionSummary>> GetSessionsAsync()
        {
            return await _db.ChatSessions
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => new ChatSessionSummary
                {
                    Id = s.Id,
                    CreatedAt = s.CreatedAt,
                    PreviewText = s.Messages
                        .OrderBy(m => m.CreatedAt)
                        .Select(m => m.Content)
                        .FirstOrDefault() ?? "Đoạn chat mới"
                })
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetMessagesAsync(Guid sessionId)
        {
            return await _db.ChatMessages
                .Where(m => m.ChatSessionId == sessionId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task AddMessageAsync(Guid sessionId, ChatRole role, string content)
        {
            var message = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatSessionId = sessionId,
                Role = role,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _db.ChatMessages.Add(message);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteSessionAsync(Guid sessionId)
        {
            var session = await _db.ChatSessions.FindAsync(sessionId);
            if (session is not null)
            {
                _db.ChatSessions.Remove(session);
                await _db.SaveChangesAsync();
            }
        }
    }
}
