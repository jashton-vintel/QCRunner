using Microsoft.EntityFrameworkCore;
using QCRunner.Core.Methods;

namespace QCRunner.Data.Repositories;

public sealed class MethodRepository : IMethodRepository
{
    private readonly QCRunnerDbContext _context;

    public MethodRepository(QCRunnerDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public async Task<IReadOnlyList<MethodSummary>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Methods
            .OrderBy(method => method.Name)
            .Select(method => new MethodSummary(method.Id, method.Code, method.Name, method.Version, method.Phases.Count))
            .ToListAsync(cancellationToken);
    }

    public Task<Method?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return _context.Methods.SingleOrDefaultAsync(method => method.Code == code, cancellationToken);
    }

    public Task<Method?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Methods.SingleOrDefaultAsync(method => method.Id == id, cancellationToken);
    }
}
