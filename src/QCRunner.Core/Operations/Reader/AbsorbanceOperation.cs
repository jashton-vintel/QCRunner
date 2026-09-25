namespace QCRunner.Core.Operations.Reader;

public class AbsorbanceOperation : MeasurementOperationBase
{
    /// <summary>Number of lamp flashes averaged for each reading.</summary>
    public int Flashes { get; set; }

    /// <summary>Delay between cycles in milliseconds.</summary>
    public int Delay { get; set; }
}
