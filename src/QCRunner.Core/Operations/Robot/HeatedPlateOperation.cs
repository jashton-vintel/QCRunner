namespace QCRunner.Core.Operations.Robot;

/// <summary>
/// Sets the temperature of one of the heated nests on the robot deck.
/// </summary>
public class HeatedPlateOperation : RobotOperationBase
{
    /// <summary>Target temperature in degrees Celsius.</summary>
    public double Temperature { get; set; }

    public int Position { get; set; }
}
