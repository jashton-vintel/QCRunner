namespace QCRunner.Core.Methods;

/// <summary>
/// A stage of a method that the operator starts explicitly, for example preparation, cold
/// testing and hot testing.
/// </summary>
public class Phase
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Index { get; set; }

    public int MethodId { get; set; }

    public virtual Method Method { get; set; } = null!;

    public virtual ICollection<OperationGroup> OperationGroups { get; set; } = new List<OperationGroup>();

    public override string ToString() => $"Phase {Index}: {Name}";
}
