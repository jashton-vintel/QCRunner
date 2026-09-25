using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Pause;

namespace QCRunner.Data.Configurations;

public sealed class PauseOperationConfiguration : IEntityTypeConfiguration<PauseOperation>
{
    public void Configure(EntityTypeBuilder<PauseOperation> builder)
    {
        builder.Property(operation => operation.DurationSeconds).HasColumnName("DurationSeconds");
        builder.Ignore(operation => operation.Duration);
    }
}
