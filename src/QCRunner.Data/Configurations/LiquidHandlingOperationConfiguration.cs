using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Robot;

namespace QCRunner.Data.Configurations;

public sealed class LiquidHandlingOperationConfiguration : IEntityTypeConfiguration<LiquidHandlingOperationBase>
{
    public void Configure(EntityTypeBuilder<LiquidHandlingOperationBase> builder)
    {
        builder.Property(operation => operation.LabwareId).HasColumnName("LabwareId");
        builder.Property(operation => operation.Volume).HasColumnName("Volume");
        builder.Property(operation => operation.Point).HasColumnName("Point").HasMaxLength(50);
        builder.Property(operation => operation.Position).HasColumnName("Position");
        builder.Property(operation => operation.MixCycles).HasColumnName("MixCycles");
        builder.Property(operation => operation.MixVolume).HasColumnName("MixVolume");
        builder.Property(operation => operation.XOffset).HasColumnName("XOffset");
        builder.Property(operation => operation.YOffset).HasColumnName("YOffset");
        builder.Property(operation => operation.ZOffset).HasColumnName("ZOffset");

        builder.HasOne(operation => operation.Plate)
            .WithMany()
            .HasForeignKey(operation => operation.LabwareId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
