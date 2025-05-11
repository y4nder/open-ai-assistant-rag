using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using OpenAI.Assistants;

namespace RagOpenAiTestApp.FunctionTools;

public interface IAgentTool
{
    string FunctionName { get; }
    
    [Experimental("OPENAI001")]
    Task ProcessAsync(AssistantClient assistantClient, ClientResult<ThreadRun> threadRun, RequiredAction action);
}