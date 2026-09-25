using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.ResultBuilding;

namespace QCRunner.Calculations.Engine;

internal abstract class LuminescenceTestBase : ReaderTestBase
{
    protected IEnumerable<LuminescenceWell> LuminescenceWells => Wells.OfType<LuminescenceWell>();

    protected LuminescenceWell GetLuminescenceWell(string name)
    {
        IWell well = GetWell(name);

        return well as LuminescenceWell
            ?? throw new ArgumentException($"{Name} expected well '{name}' to hold luminescence data but it holds {well.MeasurementType}.");
    }

    protected override void Validate()
    {
        base.Validate();

        foreach (IWell well in Wells)
        {
            if (well is not LuminescenceWell luminescenceWell || luminescenceWell.Counts.Count == 0)
            {
                well.OtherProblems = true;
                Warnings.Add($"{well.Name} has no luminescence readings");
            }
        }
    }

    protected static ResultChart BuildCountsChart(string title, IEnumerable<LuminescenceWell> wells)
    {
        var chart = new ResultChart { Title = title };
        chart.AxisX.Title = "Elapsed time [s]";
        chart.AxisX.LabelStyle.Format = "0";
        chart.AxisY.Title = "Counts";
        chart.AxisY.LabelStyle.Format = "0";

        foreach (LuminescenceWell well in wells)
        {
            var series = new Series { Name = well.Name, Type = SeriesType.ScatterLine, MarkerType = MarkerType.Circle };

            foreach (KeyValuePair<double, double> point in well.Counts.Points)
            {
                series.AddPoint(point.Key, point.Value);
            }

            chart.AddSeries(series);
        }

        return chart;
    }
}
