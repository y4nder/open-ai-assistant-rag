using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using OpenAI.Assistants;

namespace RagOpenAiTestApp.FunctionTools.ToolDefinitions;

public class GetWeatherInCelcius : IAgentTool
{
    public string FunctionName => nameof(GetWeatherInCelcius);

    [Experimental("OPENAI001")]
    public async Task ProcessAsync(AssistantClient assistantClient, ClientResult<ThreadRun> threadRun, RequiredAction action)
    {
        using JsonDocument argumentsJson = JsonDocument.Parse(action.FunctionArguments);
        bool hasCityArgument = argumentsJson.RootElement.TryGetProperty("city", out JsonElement city);

        if (!hasCityArgument)
            throw new ArgumentNullException(nameof(city), "City is required.");

        var weather = await GetWeatherInCelciusMethod(city.ToString());

        await assistantClient.SubmitToolOutputsToRunAsync(threadRun.Value.ThreadId, threadRun.Value.Id,
        [
            new ToolOutput
            {
                ToolCallId = action.ToolCallId,
                Output = weather,
            }
        ]);
    }
    
    private Task<string> GetWeatherInCelciusMethod(string city)
        => Task.FromResult(city switch
        {
            "Karachi" => "20C",
            "Lahore" => "30C",
            "Islamabad" => "25C",
            _ => "28C",
        });
}