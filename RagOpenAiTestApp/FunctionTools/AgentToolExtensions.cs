using RagOpenAiTestApp.FunctionTools.ToolDefinitions;

namespace RagOpenAiTestApp.FunctionTools;

public static class AgentToolExtensions
{
    public static IServiceCollection AddAgentTools(this IServiceCollection services)
    {
        services.AddScoped<ToolRouter>();
        services.AddScoped<IAgentTool, GetWeatherInCelcius>();
        return services;
    }
}