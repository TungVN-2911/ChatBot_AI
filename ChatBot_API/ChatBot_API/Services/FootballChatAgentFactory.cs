using ChatBot_Shared.Search;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Data;

namespace ChatBot_API.Services
{
    public class FootballChatAgentFactory
    {
        private readonly Kernel _kernel;
        private readonly PineconeTextSearch _pineconeTextSearch;
        private readonly ILoggerFactory _loggerFactory;
        private readonly GraphQueryPlugin _graphQueryPlugin;

        public FootballChatAgentFactory(Kernel kernel, PineconeTextSearch pineconeTextSearch, ILoggerFactory loggerFactory, GraphQueryPlugin graphQueryPlugin)
        {
            _kernel = kernel;
            _pineconeTextSearch = pineconeTextSearch;
            _loggerFactory = loggerFactory;
            _graphQueryPlugin = graphQueryPlugin;
            _kernel.Plugins.AddFromObject(_graphQueryPlugin, "GraphQuery");

        }
        public ChatCompletionAgent CreateAgent()
        {
            var agent = new ChatCompletionAgent
            {
                Name = "ChampionLeagueAssistant",
                Instructions = "You are a football assistant that answers ONLY using the context provided below. " +
                               "Follow these rules strictly:\n" +
                               "1. Base your answer ONLY on the given context. Never use outside knowledge, even if you know the answer.\n" +
                               "2. If the context is empty or does not contain enough information to answer confidently, " +
                               "respond exactly: \"I don't have enough information to answer that.\"\n" +
                               "3. Never guess, speculate, or make up names, scores, or dates.\n" +
                               "4. If the question is unrelated to Premier League or Champions League football, decline politely.",
                Kernel = _kernel,
                Arguments = new KernelArguments(new GeminiPromptExecutionSettings
                {
                    Temperature = 0.1,
                    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
                })
            };
            return agent;
        }

#pragma warning disable SKEXP0110, SKEXP0130
        public ChatHistoryAgentThread CreateThread(ChatHistory? existingHistory = null)
        {
            var thread = existingHistory is null
                ? new ChatHistoryAgentThread()
                : new ChatHistoryAgentThread(existingHistory, Guid.NewGuid().ToString());

            thread.AIContextProviders.Add(
                new TextSearchProvider(_pineconeTextSearch, _loggerFactory, new TextSearchProviderOptions { Top = 5 }));
            return thread;
        }
#pragma warning restore SKEXP0110, SKEXP0130
    }
}
