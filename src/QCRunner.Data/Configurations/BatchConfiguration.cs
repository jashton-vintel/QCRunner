using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Batches;

namespace QCRunner.Data.Configurations;

public sealed class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.ToTable("Batches");
        builder.HasKey(batch => batch.Id);
        builder.Property(batch => batch.Reference).HasMaxLength(100).IsRequired();
        builder.Property(batch => batch.KitLotNumber).HasMaxLength(100);
        builder.Property(batch => batch.OperatorName).HasMaxLength(200);
        builder.Property(batch => batch.Comments).HasMaxLength(4000);
        builder.Property(batch => batch.ReaderSerialNumber).HasMaxLength(100);
        builder.Property(batch => batch.RobotSerialNumber).HasMaxLength(100);
        builder.HasIndex(batch => batch.Reference);

        builder.HasOne(batch => batch.Method)
            .WithMany()
            .HasForeignKey(batch => batch.MethodId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
