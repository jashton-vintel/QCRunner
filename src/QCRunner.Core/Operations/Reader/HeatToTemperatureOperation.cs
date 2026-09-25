namespace QCRunner.Core.Operations.Reader;

public class HeatToTemperatureOperation : ReaderOperationBase
{
    /// <summary>Target temperature in degrees Celsius.</summary>
    public double Temperature { get; set; }

    public HeatMode Mode { get; set; }
}
