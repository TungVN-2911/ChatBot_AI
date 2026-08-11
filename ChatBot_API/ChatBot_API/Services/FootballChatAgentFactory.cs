using ChatBot_Shared.LLM;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Data;

namespace ChatBot_API.Services
{
    public class FootballChatAgentFactory
    {
        private readonly Kernel _kernel;
        private readonly PineconeTextSearch _pineconeTextSearch;
        private readonly ILoggerFactory _loggerFactory;

        public FootballChatAgentFactory(Kernel kernel, PineconeTextSearch pineconeTextSearch, ILoggerFactory loggerFactory)
        {
            _kernel = kernel;
            _pineconeTextSearch = pineconeTextSearch;
            _loggerFactory = loggerFactory;
        }
        public ChatCompletionAgent CreateAgent()
        {
            var agent = new ChatCompletionAgent
            {
                Name = "ChampionLeagueAssistant",
                Instructions = "You are a helpful assistant that answers questions about " +
                    "Champions League football using the provided context. " +
                    "If the context does not contain the answer, say you don't know instead of guessing.",
                Kernel = _kernel
            };
            return agent;
        }

#pragma warning disable SKEXP0110, SKEXP0130
        public ChatHistoryAgentThread CreateThread()
        {
            var thread = new ChatHistoryAgentThread();
            thread.AIContextProviders.Add(
                new TextSearchProvider(_pineconeTextSearch, _loggerFactory, new TextSearchProviderOptions { Top = 5 }));
            return thread;
        }
#pragma warning restore SKEXP0110, SKEXP0130
    }
}
