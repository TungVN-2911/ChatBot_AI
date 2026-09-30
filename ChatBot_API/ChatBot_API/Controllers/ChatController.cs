using ChatBot_API.Dtos;
using ChatBot_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot_API.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly FootballChatAgentFactory _agentFactory;

        public ChatController(FootballChatAgentFactory agentFactory)
        {
            _agentFactory = agentFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var agent = _agentFactory.CreateAgent();
            var thread = _agentFactory.CreateThread();

            var answer = new List<string>();
            await foreach (var item in agent.InvokeAsync(request.Question, thread))
            {
                if (item.Message.Content is not null)
                {
                    answer.Add(item.Message.Content);
                }
            }

            return Ok(new { Answer = string.Join(" ", answer) });
        }
    }
}
