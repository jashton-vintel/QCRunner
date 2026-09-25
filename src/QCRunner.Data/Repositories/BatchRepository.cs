using QCRunner.Core.Batches;

namespace QCRunner.Data.Repositories;

public sealed class BatchRepository : IBatchRepository
{
    private readonly QCRunnerDbContext _context;

    public BatchRepository(QCRunnerDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public async Task AddAsync(Batch batch, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(batch);

        await _context.Batches.AddAsync(batch, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
