namespace QCRunner.Core.Methods;

/// <summary>
/// The fields a method picker needs, without loading the method body.
/// </summary>
public sealed record MethodSummary(int Id, string Code, string Name, string Version, int PhaseCount);
