using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations;
using QCRunner.Core.Operations.Calculation;
using QCRunner.Core.Operations.Pause;
using QCRunner.Core.Operations.Prompt;
using QCRunner.Core.Operations.Reader;
using QCRunner.Core.Operations.Robot;

namespace QCRunner.Data.Configurations;

/// <summary>
/// Every operation type shares one table. The discriminator names the concrete type so a
/// method loads back as the same classes the runners switch on.
/// </summary>
public sealed class OperationConfiguration : IEntityTypeConfiguration<OperationBase>
{
    public void Configure(EntityTypeBuilder<OperationBase> builder)
    {
        builder.ToTable("Operations");
        builder.HasKey(operation => operation.Id);
        builder.Property(operation => operation.Name).HasMaxLength(200).IsRequired();
        builder.Property(operation => operation.Description).HasMaxLength(1000);
        builder.Ignore(operation => operation.Type);
        builder.HasIndex(operation => new { operation.OperationGroupId, operation.Index });

        builder.HasDiscriminator<string>("OperationKind")
            .HasValue<AbsorbanceOperation>("Absorbance")
            .HasValue<LuminescenceOperation>("Luminescence")
            .HasValue<ShakeOperation>("Shake")
            .HasValue<HeatToTemperatureOperation>("HeatToTemperature")
            .HasValue<AspirateOperation>("Aspirate")
            .HasValue<DispenseOperation>("Dispense")
            .HasValue<HeatedPlateOperation>("HeatedPlate")
            .HasValue<CalculationOperation>("Calculation")
            .HasValue<PromptOperation>("Prompt")
            .HasValue<PauseOperation>("Pause");
    }
}
