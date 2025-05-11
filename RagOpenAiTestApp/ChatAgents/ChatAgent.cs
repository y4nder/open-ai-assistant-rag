using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using OpenAI.Assistants;
using RagOpenAiTestApp.DTOs;
using RagOpenAiTestApp.FunctionTools;

namespace RagOpenAiTestApp.ChatAgents;

public interface IChatAgent
{
    Task<(string Response, string ThreadId)> GetResponseAsync(PromptDto dto);
}

[Experimental("OPENAI001")]
public class ChatAgent : IChatAgent
{
    private readonly AssistantClient _assistantClient;
    private readonly ToolRouter _toolRouter;
    private readonly string _assistantId;

    public ChatAgent(AssistantClient assistantClient, ToolRouter toolRouter, IOptions<OpenAiSettings> settings)
    {
        _assistantClient = assistantClient;
        _toolRouter = toolRouter;
        _assistantId = settings.Value.AssistantId;
    }

    public async Task<(string Response, string ThreadId)> GetResponseAsync(PromptDto dto)
    {
        ClientResult<ThreadRun> threadRun;

        if (!string.IsNullOrEmpty(dto.ThreadId))
            threadRun = await _assistantClient.CreateRunAsync(dto.ThreadId, _assistantId,
                new RunCreationOptions
                {
                    AdditionalMessages = { dto.PromptMessage },
                });
        else
            threadRun = await _assistantClient.CreateThreadAndRunAsync(_assistantId, new ThreadCreationOptions
            {
                InitialMessages = { dto.PromptMessage }
            });

        await ProcessRequiredActions(_assistantClient, threadRun);

        var messages = _assistantClient.GetMessages(threadRun.Value.ThreadId);
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
    
    private async Task ProcessRequiredActions(
        AssistantClient assistantClient,
        ClientResult<ThreadRun> threadRun)
    {
        while (!threadRun.Value.Status.IsTerminal)
        {
            if (threadRun.Value.Status == RunStatus.RequiresAction)
            {
                foreach (var action in threadRun.Value.RequiredActions)
                {
                    
                    await _toolRouter.RouteToolAsync(assistantClient, threadRun, action);
                }
            }
            await Task.Delay(500);
            threadRun = await assistantClient.GetRunAsync(threadRun.Value.ThreadId, threadRun.Value.Id);
        }
    }
}