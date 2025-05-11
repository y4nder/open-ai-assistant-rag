namespace RagOpenAiTestApp.DTOs;

public class PromptDto
{
    public string PromptMessage { get; set; } = null!;
    public string? ThreadId { get; set; } = string.Empty;
}

// public class PromptRequestDto
// {
//     public string? ThreadId { get; set; }
//     public string PromptMessage { get; set; } = null!;
//     public PromptContext? Context { get; set; }
// }
//
// public class PromptContext
// {
//     public Guid? NoteId { get; set; }
//     public Guid? FolderId { get; set; }
// }
//
// public class PromptResponseDto
// {
//     public string? ThreadId { get; set; } = string.Empty;
//     public string Message { get; set; } = string.Empty;
//     public List<PromptResponseCitation> Citations { get; set; } = new();
// }
//
// public class PromptResponseCitation
// {
//     public string Type { get; set; } = null!;
//     public Guid Id { get; set; }
// }