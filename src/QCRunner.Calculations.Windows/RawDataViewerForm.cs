namespace QCRunner.Calculations.Windows;

/// <summary>
/// Placeholder for the raw calculation data viewer, which charts the wells fed into a
/// calculation and the result objects that come back from QCRunner.Calculations.
/// </summary>
public sealed class RawDataViewerForm : Form
{
    public RawDataViewerForm()
    {
        Text = "QCRunner calculation data viewer";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(640, 360);

        Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "Raw calculation data viewer shell."
        });
    }
}
