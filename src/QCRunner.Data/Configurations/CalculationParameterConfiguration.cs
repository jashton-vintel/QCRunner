using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Calculation;

namespace QCRunner.Data.Configurations;

public sealed class CalculationParameterConfiguration : IEntityTypeConfiguration<CalculationParameter>
{
    public void Configure(EntityTypeBuilder<CalculationParameter> builder)
    {
        builder.ToTable("CalculationParameters");
        builder.HasKey(parameter => parameter.Id);
        builder.Property(parameter => parameter.Name).HasMaxLength(100).IsRequired();
    }
}
