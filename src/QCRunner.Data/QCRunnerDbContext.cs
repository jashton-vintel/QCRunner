using Microsoft.EntityFrameworkCore;
using QCRunner.Core.Batches;
using QCRunner.Core.Methods;
using QCRunner.Core.Operations;

namespace QCRunner.Data;

/// <summary>
/// The persistence model. Entity shapes live in Core; everything database specific is in the
/// configuration classes next to this context.
/// </summary>
public sealed class QCRunnerDbContext : DbContext
{
    public QCRunnerDbContext(DbContextOptions<QCRunnerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Method> Methods => Set<Method>();

    public DbSet<Phase> Phases => Set<Phase>();

    public DbSet<OperationGroup> OperationGroups => Set<OperationGroup>();

    public DbSet<OperationBase> Operations => Set<OperationBase>();

    public DbSet<Core.Labware.Labware> Labware => Set<Core.Labware.Labware>();

    public DbSet<Batch> Batches => Set<Batch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QCRunnerDbContext).Assembly);
    }
}
