using System.Diagnostics.CodeAnalysis;

namespace RagOpenAiTestApp.ChatAgents;

public static class ChatAgentExtensions
{
    [Experimental("OPENAI001")]
    public static IServiceCollection AddChatAgent(this IServiceCollection services)
    {
        services.AddScoped<IChatAgent, ChatAgent>();
        return services;
    }
}