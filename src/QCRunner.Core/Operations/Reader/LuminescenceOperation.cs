namespace QCRunner.Core.Operations.Reader;

public class LuminescenceOperation : MeasurementOperationBase
{
    /// <summary>How long the photomultiplier counts each well.</summary>
    public TimeSpan CountingTime { get; set; }
}
