using Microsoft.Extensions.Logging;
using QCRunner.Calculations.Common;
using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Core.Operations.Calculation;
using QCRunner.Core.ReaderData;

namespace QCRunner.Core.Calculations;

public sealed class CalculationProcessor : ICalculationProcessor
{
    private readonly ICalculationEngine _engine;
    private readonly ILogger<CalculationProcessor> _logger;

    public CalculationProcessor(ICalculationEngine engine, ILogger<CalculationProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(engine);
        ArgumentNullException.ThrowIfNull(logger);

        _engine = engine;
        _logger = logger;
    }

    public ICalculationResult Calculate(CalculationOperation operation, IReadOnlyList<WellReaderData> wellData)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(wellData);

        List<IWell> wells = wellData.Select(ToWell).ToList();
        var parameters = new TestParameters(operation.Parameters.Select(parameter => new KeyValuePair<string, double>(parameter.Name, parameter.Value)));

        _logger.LogInformation("Calculating {Calculation} from {WellCount} wells and {ParameterCount} parameters",
            operation.CalculationType, wells.Count, parameters.Values.Count);

        ICalculationResult result = _engine.Calculate(operation.CalculationType, wells, parameters);

        _logger.LogInformation("{Calculation} result: {ReportValue} ({Outcome})", result.Name, result.ReportValue, result.Result);

        return result;
    }

    private static IWell ToWell(WellReaderData data)
    {
        CalculationWell well = data.Well;

        return well.DataType switch
        {
            ReaderDataType.Absorbance => ToAbsorbanceWell(well, data.Readings.OfType<AbsorbanceData>().Last()),
            ReaderDataType.Luminescence => ToLuminescenceWell(well, data.Readings.OfType<LuminescenceData>()),
            _ => throw new NotSupportedException($"Reader data of type {well.DataType} cannot be converted into a calculation well.")
        };
    }

    private static AbsorbanceWell ToAbsorbanceWell(CalculationWell well, AbsorbanceData reading)
    {
        var absorbanceWell = new AbsorbanceWell(well.Name, well.Role, well.Code, well.TrueValue ?? double.NaN);

        for (int index = 0; index < reading.Values.Count; index++)
        {
            absorbanceWell.Spectrum.Add(reading.WavelengthAt(index), reading.Values[index]);
        }

        return absorbanceWell;
    }

    private static LuminescenceWell ToLuminescenceWell(CalculationWell well, IEnumerable<LuminescenceData> readings)
    {
        var luminescenceWell = new LuminescenceWell(well.Name, well.Role, well.Code, well.TrueValue ?? double.NaN);

        foreach (LuminescenceData reading in readings.OrderBy(reading => reading.ElapsedTime))
        {
            luminescenceWell.Counts.Add(reading.ElapsedTime.TotalSeconds, reading.Counts);
        }

        return luminescenceWell;
    }
}
