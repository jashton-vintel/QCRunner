namespace QCRunner.Windows;

/// <summary>
/// Placeholder for the method designer, where users build phases, operation groups and
/// operations. The domain it would edit lives in QCRunner.Core and is persisted by QCRunner.Data.
/// </summary>
public sealed class MethodDesignerForm : Form
{
    public MethodDesignerForm()
    {
        Text = "QCRunner method designer";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(640, 360);

        Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "Method designer shell. See QCRunner.Demo for the batch running workflow."
        });
    }
}
