using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Robot;

namespace QCRunner.Data.Configurations;

public sealed class HeatedPlateOperationConfiguration : IEntityTypeConfiguration<HeatedPlateOperation>
{
    public void Configure(EntityTypeBuilder<HeatedPlateOperation> builder)
    {
        builder.Property(operation => operation.Temperature).HasColumnName("Temperature");
        builder.Property(operation => operation.Position).HasColumnName("Position");
    }
}
