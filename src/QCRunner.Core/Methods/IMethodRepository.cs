namespace QCRunner.Core.Methods;

public interface IMethodRepository
{
    Task<IReadOnlyList<MethodSummary>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads a method by its code. Phases, groups and operations are loaded lazily as the
    /// runner walks them, so the repository must outlive the batch run.
    /// </summary>
    Task<Method?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<Method?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
