using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Assistants;

namespace RagOpenAiTestApp;

public static class OpenAiExtensions
{
    [Experimental("OPENAI001")]
    public static IServiceCollection AddOpenAi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenAiSettings>(
            configuration.GetSection(nameof(OpenAiSettings))
        );
        
        services.AddScoped<OpenAIClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<OpenAiSettings>>();
            return new OpenAIClient(settings.Value.ApiKey);

        });
        
        services.AddScoped<AssistantClient>(sp =>
        {
            var client = sp.GetRequiredService<OpenAIClient>();
            return client.GetAssistantClient();
        }); 
        return services;
    }
}

public class OpenAiSettings
{
    public string ApiKey { get; init; } = null!;
    public string AssistantId { get; init; } = null!;
}