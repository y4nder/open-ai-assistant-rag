using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Carter;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Assistants;
using RagOpenAiTestApp.DTOs;

namespace RagOpenAiTestApp;

public class ChatModule : ICarterModule
{
    const string AssistantId = "asst_ERFNUBrB32Pc2FYweAXcTCEl";
    
    [Experimental("OPENAI001")]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/chat", async (PromptDto promptDto, [FromServices] AssistantClient assistantClient) =>
        {
            try
            {
                var (response, threadId) = await GetResponseAsync(assistantClient, promptDto.PromptMessage, promptDto.ThreadId);
                return Results.Ok(new { reply = response, threadId });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
    
    [Experimental("OPENAI001")]
    private async Task<(string Response, string ThreadId)> GetResponseAsync(AssistantClient assistantClient, string message, string? threadId = null)
    {   
        ClientResult<ThreadRun> threadRun;

        if (!string.IsNullOrEmpty(threadId))
            threadRun = await assistantClient.CreateRunAsync(threadId, AssistantId,
                        new RunCreationOptions
                        {
                            AdditionalMessages = { message },
                        });
        else
            threadRun = await assistantClient.CreateThreadAndRunAsync(AssistantId, new ThreadCreationOptions
            {
                InitialMessages = { message }
            });

        await ProcessRequiredActions(assistantClient, threadRun);

        var messages = assistantClient.GetMessages(threadRun.Value.ThreadId);
        var messageItem = messages.First();

        string responseText = "";
        foreach (var content in messageItem.Content)
        {
            var text = content.Text;
            foreach (var annotation in content.TextAnnotations)
                text = text.Replace(annotation.TextToReplace, "");
            
            responseText += text;
        }   

        return (responseText, threadRun.Value.ThreadId);
    }

    [Experimental("OPENAI001")]
    private async Task ProcessRequiredActions(AssistantClient assistantClient, ClientResult<ThreadRun> threadRun)
    {
        while (!threadRun.Value.Status.IsTerminal)
        {
            if (threadRun.Value.Status == RunStatus.RequiresAction)
            {
                foreach (var action in threadRun.Value.RequiredActions)
                {
                    
                    switch (action.FunctionName)
                    {
                        case nameof(GetWeatherInCelcius):
                            await ProcessCityWeatherAction(assistantClient, threadRun, action);
                            break;
                    }
                }
            }


            await Task.Delay(500);
            threadRun = await assistantClient.GetRunAsync(threadRun.Value.ThreadId, threadRun.Value.Id);
        }
    }

    [Experimental("OPENAI001")]
    private async Task ProcessCityWeatherAction(AssistantClient assistantClient, ClientResult<ThreadRun> threadRun,
        RequiredAction action)
    {
        using JsonDocument argumentsJson = JsonDocument.Parse(action.FunctionArguments);
        bool hasCityArgument = argumentsJson.RootElement.TryGetProperty("city", out JsonElement city);

        if (!hasCityArgument)
            throw new ArgumentNullException(nameof(city), "City is required.");

        var weather = await GetWeatherInCelcius(city.ToString());

        await assistantClient.SubmitToolOutputsToRunAsync(threadRun.Value.ThreadId, threadRun.Value.Id,
        [
            new ToolOutput
            {
                ToolCallId = action.ToolCallId,
                Output = weather,
            }
        ]);
    }


    private Task<string> GetWeatherInCelcius(string city)
        => Task.FromResult(city switch
        {
            "Karachi" => "20C",
            "Lahore" => "30C",
            "Islamabad" => "25C",
            _ => "28C",
        });
}