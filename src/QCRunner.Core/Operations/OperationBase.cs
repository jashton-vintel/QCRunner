using QCRunner.Core.Methods;

namespace QCRunner.Core.Operations;

/// <summary>
/// Root of the operation hierarchy. Every concrete operation is persisted in a single table
/// and materialised back as its own type, so runners can switch on the CLR type.
/// </summary>
public abstract class OperationBase : IOperation
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Index { get; set; }

    public abstract OperationType Type { get; }

    public int OperationGroupId { get; set; }

    public virtual OperationGroup? OperationGroup { get; set; }

    public override string ToString() => $"{Type} operation {Index}: {Name}";
}
