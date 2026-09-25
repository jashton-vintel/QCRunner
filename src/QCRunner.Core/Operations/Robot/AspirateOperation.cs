namespace QCRunner.Core.Operations.Robot;

public class AspirateOperation : LiquidHandlingOperationBase
{
    /// <summary>Height above the well bottom at which liquid is drawn, in millimetres.</summary>
    public double AspirateHeight { get; set; }
}
