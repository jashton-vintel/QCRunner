namespace QCRunner.Core.Methods;

/// <summary>
/// An analytic method: the ordered phases, operation groups and operations that drive the
/// hardware, collect data and evaluate the results for one kind of product.
/// </summary>
public class Method
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public bool Published { get; set; }

    public virtual ICollection<Phase> Phases { get; set; } = new List<Phase>();

    public override string ToString() => $"{Name} ({Code} v{Version})";
}
