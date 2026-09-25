namespace QCRunner.Calculations.Common.Results;

public interface ICoordinate
{
    double X { get; }

    double Y { get; }

    bool IsEmpty { get; }
}
