using QCRunner.Core.Operations;

namespace QCRunner.Core.Methods;

/// <summary>
/// A named run of operations inside a phase, kept together so the method designer can move
/// and reuse them as a unit.
/// </summary>
public class OperationGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Index { get; set; }

    public int PhaseId { get; set; }

    public virtual Phase Phase { get; set; } = null!;

    public virtual ICollection<OperationBase> Operations { get; set; } = new List<OperationBase>();

    public override string ToString() => $"Group {Index}: {Name}";
}
