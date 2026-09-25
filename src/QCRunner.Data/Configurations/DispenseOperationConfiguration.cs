using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Robot;

namespace QCRunner.Data.Configurations;

public sealed class DispenseOperationConfiguration : IEntityTypeConfiguration<DispenseOperation>
{
    public void Configure(EntityTypeBuilder<DispenseOperation> builder)
    {
        builder.Property(operation => operation.DispenseHeight).HasColumnName("DispenseHeight");
    }
}
