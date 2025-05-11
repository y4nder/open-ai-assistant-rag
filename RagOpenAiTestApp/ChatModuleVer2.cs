using System.Diagnostics.CodeAnalysis;
using Carter;
using Microsoft.AspNetCore.Mvc;
using RagOpenAiTestApp.ChatAgents;
using RagOpenAiTestApp.DTOs;

namespace RagOpenAiTestApp;

public class ChatModuleVer2 : ICarterModule 
{
    [Experimental("OPENAI001")]
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/chat/v2", async (PromptDto promptDto, [FromServices]IChatAgent chatAgent) =>
        {
            try
            {
                var (response, threadId) = await chatAgent.GetResponseAsync(promptDto); 
                return Results.Ok(new { reply = response, threadId });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}