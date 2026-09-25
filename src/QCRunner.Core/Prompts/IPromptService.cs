namespace QCRunner.Core.Prompts;

/// <summary>
/// Shows a message to the operator and waits for an answer. Runners call this from worker
/// threads, so a WinForms implementation must marshal onto the UI thread (Control.Invoke)
/// before it touches a control.
/// </summary>
public interface IPromptService
{
    Task<PromptResponse> ShowPromptAsync(PromptRequest request, CancellationToken cancellationToken = default);
}
