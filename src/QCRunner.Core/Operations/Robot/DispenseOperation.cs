namespace QCRunner.Core.Operations.Robot;

public class DispenseOperation : LiquidHandlingOperationBase
{
    /// <summary>Height above the well bottom at which liquid is released, in millimetres.</summary>
    public double DispenseHeight { get; set; }
}
