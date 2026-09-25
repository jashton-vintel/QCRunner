using QCRunner.Calculations.Common;
using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.ResultBuilding;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Engine;

/// <summary>
/// A test fed by plate reader wells. Wells are keyed by the name the method designer gave
/// them; a required well is satisfied by a supplied well with the same name or, failing that,
/// any supplied well in the same role.
/// </summary>
internal abstract class ReaderTestBase : TestBase
{
    private readonly Dictionary<string, IWell> _wells = new(StringComparer.OrdinalIgnoreCase);

    protected IEnumerable<IWell> Wells => _wells.Values;

    protected override void LoadData(IEnumerable<ITestData> data)
    {
        _wells.Clear();

        foreach (ITestData item in data)
        {
            if (item is not IWell well)
            {
                throw new ArgumentException($"{Name} only accepts plate reader wells but received {item.GetType().Name}.", nameof(data));
            }

            _wells[well.Name] = well;
        }
    }

    protected override void Validate()
    {
        List<string> missing = WellsNeeded
            .Where(needed => !_wells.ContainsKey(needed.Name) && !Wells.Any(well => well.Role == needed.Role))
            .Select(needed => $"{needed.Name} ({needed.Role})")
            .ToList();

        if (missing.Count > 0)
        {
            throw new InvalidWellsException(CalculationType, missing);
        }
    }

    protected IWell GetWell(string name)
    {
        if (_wells.TryGetValue(name, out IWell? named))
        {
            return named;
        }

        IWell? needed = WellsNeeded.FirstOrDefault(well => well.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        IWell? byRole = needed is null ? null : Wells.FirstOrDefault(well => well.Role == needed.Role);

        return byRole ?? throw new InvalidWellsException(CalculationType, new[] { name });
    }

    protected AcceptanceWindow CreateAcceptanceWindow(Unit unit, int decimals)
    {
        var lower = LowerSpecificationLimit.Closed(new DimensionedValue(Parameters.Get("LowerLimit"), unit), decimals);
        var upper = UpperSpecificationLimit.Closed(new DimensionedValue(Parameters.Get("UpperLimit"), unit), decimals);

        return new AcceptanceWindow(lower, upper);
    }

    protected CalculationAssayResult Assess(AcceptanceWindow window, IDimensionedValue measured)
    {
        if (Wells.Any(well => !well.IsValid) || double.IsNaN(measured.Value))
        {
            return CalculationAssayResult.Invalid;
        }

        return window.Contains(measured) ? CalculationAssayResult.Pass : CalculationAssayResult.Fail;
    }

    protected ResultTable BuildSuitabilityTable()
    {
        var table = new ResultTable("Suitability", "Well", "Role", "Position", "Status");

        foreach (IWell well in Wells.OrderBy(well => well.Code))
        {
            table.AddRow(well.Name, well.Role.ToString(), well.Code.ToString(), well.IsValid ? "Valid" : "Excluded");
        }

        return table;
    }

    protected static ResultTable BuildMetricsTable(string metric, AcceptanceWindow window, IDimensionedValue measured, CalculationAssayResult outcome)
    {
        var table = new ResultTable("Metrics", "Metric", "Acceptance criteria", "Measured value", "Result");
        table.AddRow(metric, window.ToString(), measured.ToString(window.Format), outcome.ToString());

        return table;
    }
}
