namespace QCRunner.Core.Batches;

/// <summary>
/// The release state a reviewer assigns after the batch has run.
/// </summary>
public enum BatchStatus
{
    Unreleased = 0,
    Released = 1,
    Rejected = 2
}
