using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using OpenAI.Assistants;

namespace RagOpenAiTestApp.FunctionTools;

public class ToolRouter
{
    private readonly IEnumerable<IAgentTool> _agentTools;

    public ToolRouter(IEnumerable<IAgentTool> agentTools)
    {
        _agentTools = agentTools;
    }

    [Experimental("OPENAI001")]
    public async Task RouteToolAsync(AssistantClient assistantClient, ClientResult<ThreadRun> threadRun,
        RequiredAction action)
    {
        var tool = _agentTools.FirstOrDefault(x => x.FunctionName == action.FunctionName);
        if (tool == null)
            throw new ArgumentNullException(nameof(tool), "Tool not found.");
        
        await tool.ProcessAsync(assistantClient, threadRun, action);
    }
}