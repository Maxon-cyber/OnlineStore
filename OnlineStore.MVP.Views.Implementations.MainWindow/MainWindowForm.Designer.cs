namespace OnlineStore.MVP.Views.Implementations.MainWindow;

public sealed partial class MainWindowForm
{
    private readonly System.ComponentModel.IContainer _components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (_components != null))
            _components.Dispose();

        base.Dispose(disposing);
    }

    private void AddSection(UserControl control)
    {
        control.BorderStyle = BorderStyle.None;
        control.Dock = DockStyle.Fill;

        contentPanel.Controls.Add(control);
        contentPanel.Tag = control;

        control.BringToFront();
    }

    #region Windows Form Designer generated code
    private void InitializeComponent()
    {
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "MainForm";
    }

    #endregion
}