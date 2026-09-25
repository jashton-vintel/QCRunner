using QCRunner.Core.Operations.Prompt;

namespace QCRunner.Core.Prompts;

public sealed record PromptRequest(string Title, string Message, PromptButtons Buttons);
