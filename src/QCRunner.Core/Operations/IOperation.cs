namespace QCRunner.Core.Operations;

/// <summary>
/// One step of a method. Operations are stored with the method and executed in
/// <see cref="Index"/> order within their operation group.
/// </summary>
public interface IOperation
{
    int Id { get; }

    string Name { get; }

    string Description { get; }

    int Index { get; }

    OperationType Type { get; }
}
