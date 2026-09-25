namespace QCRunner.Core.Operations.Prompt;

/// <summary>
/// Stops the method until the operator answers a message. A Cancel answer ends the batch.
/// </summary>
public class PromptOperation : OperationBase
{
    public override OperationType Type => OperationType.Prompt;

    public string MessageTitle { get; set; } = string.Empty;

    public string MessageText { get; set; } = string.Empty;

    public PromptButtons Buttons { get; set; } = PromptButtons.OkOnly;
}
