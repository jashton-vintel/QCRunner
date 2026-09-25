namespace QCRunner.Core.Batches;

public interface IBatchRepository
{
    Task AddAsync(Batch batch, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
