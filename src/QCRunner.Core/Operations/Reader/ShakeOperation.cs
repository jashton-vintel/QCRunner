namespace QCRunner.Core.Operations.Reader;

public class ShakeOperation : PlateOperationBase
{
    public ShakeIntensity Intensity { get; set; }

    public TimeSpan ShakeTime { get; set; }

    public int ShakeCycles { get; set; } = 1;
}
