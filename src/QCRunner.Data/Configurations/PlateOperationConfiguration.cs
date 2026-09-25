using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Reader;

namespace QCRunner.Data.Configurations;

public sealed class PlateOperationConfiguration : IEntityTypeConfiguration<PlateOperationBase>
{
    public void Configure(EntityTypeBuilder<PlateOperationBase> builder)
    {
        builder.Property(operation => operation.LabwareId).HasColumnName("LabwareId");

        builder.HasOne(operation => operation.Plate)
            .WithMany()
            .HasForeignKey(operation => operation.LabwareId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
