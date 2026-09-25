using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.ResultBuilding;

namespace QCRunner.Calculations.Engine;

internal abstract class AbsorbanceTestBase : ReaderTestBase
{
    protected IEnumerable<AbsorbanceWell> AbsorbanceWells => Wells.OfType<AbsorbanceWell>();

    protected AbsorbanceWell GetAbsorbanceWell(string name)
    {
        IWell well = GetWell(name);

        return well as AbsorbanceWell
            ?? throw new ArgumentException($"{Name} expected well '{name}' to hold absorbance data but it holds {well.MeasurementType}.");
    }

    protected override void Validate()
    {
        base.Validate();

        foreach (IWell well in Wells)
        {
            if (well is not AbsorbanceWell absorbanceWell || absorbanceWell.Spectrum.Count == 0)
            {
                well.OtherProblems = true;
                Warnings.Add($"{well.Name} has no absorbance spectrum");
            }
        }
    }

    protected static ResultChart BuildSpectrumChart(string title, IEnumerable<AbsorbanceWell> wells)
    {
        var chart = new ResultChart { Title = title };
        chart.AxisX.Title = "Wavelength [nm]";
        chart.AxisX.LabelStyle.Format = "0";
        chart.AxisY.Title = "Absorbance [AU]";
        chart.AxisY.LabelStyle.Format = "0.000";

        foreach (AbsorbanceWell well in wells)
        {
            var series = new Series { Name = well.Name, Type = SeriesType.Spline };

            foreach (KeyValuePair<double, double> point in well.Spectrum.Points)
            {
                series.AddPoint(point.Key, point.Value);
            }

            chart.AddSeries(series);
        }

        return chart;
    }
}
