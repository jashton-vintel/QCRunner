using QCRunner.Core.Operations.Prompt;
using QCRunner.Core.Prompts;

namespace QCRunner.Core.Runners;

public sealed class PromptOperationResult : OperationResult
{
    public PromptOperationResult(PromptOperation operation, PromptResponse response)
        : base(operation, response == PromptResponse.Cancel ? OperationResultStatus.Cancelled : OperationResultStatus.Successful, $"Operator answered {response}.")
    {
        Response = response;
    }

    public PromptResponse Response { get; }
}
