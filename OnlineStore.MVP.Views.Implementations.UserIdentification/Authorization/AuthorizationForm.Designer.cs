using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace OnlineStore.MVP.Views.Implementations.UserIdentification.Authorization;

public sealed partial class AuthorizationForm
{
    private readonly System.ComponentModel.IContainer _components = null;

    private const int BORDER_RADIUS = 20;
    private const int BORDER_SIZE = 2;

    private readonly Color _borderColor = Color.FromArgb(128, 128, 255);

    private struct FormBoundsColors
    {
        public Color TopLeftColor;
        public Color TopRightColor;
        public Color BottomLeftColor;
        public Color BottomRightColor;
    }
    
    [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
    private extern static void ReleaseCapture();
    
    [DllImport("user32.DLL", EntryPoint = "SendMessage")]
    private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.Style |= 0x20000;

            return cp;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (_components != null))
            _components.Dispose();

        base.Dispose(disposing);
    }

    private GraphicsPath GetRoundedPath(Rectangle rect, float radius)
    {
        GraphicsPath path = new GraphicsPath();
        float curveSize = radius * 2F;

        path.StartFigure();

        path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
        path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
        path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
        path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
        path.CloseFigure();
        
        return path;
    }

    private void FormRegionAndBorder(Form form, float radius, Graphics graph, Color borderColor, float borderSize)
    {
        if (WindowState != FormWindowState.Minimized)
        {
            using GraphicsPath roundPath = GetRoundedPath(form.ClientRectangle, radius);
            using Pen penBorder = new Pen(borderColor, borderSize);
            using Matrix transform = new Matrix();
            
            graph.SmoothingMode = SmoothingMode.AntiAlias;
            form.Region = new Region(roundPath);

            if (borderSize >= 1)
            {
                Rectangle rect = form.ClientRectangle;
                float scaleX = 1.0F - ((borderSize + 1) / rect.Width);
                float scaleY = 1.0F - ((borderSize + 1) / rect.Height);
                transform.Scale(scaleX, scaleY);
                transform.Translate(borderSize / 1.6F, borderSize / 1.6F);
                graph.Transform = transform;

                graph.DrawPath(penBorder, roundPath);
            }
        }
    }

    private void DrawPath(Rectangle rect, Graphics graph, Color color)
    {
        using GraphicsPath roundPath = GetRoundedPath(rect, BORDER_RADIUS);
        using Pen penBorder = new Pen(color, 3);

        graph.DrawPath(penBorder, roundPath);
    }

    private void ControlRegionAndBorder(Control control, float radius, Graphics graph, Color borderColor)
    {
        using GraphicsPath roundPath = GetRoundedPath(control.ClientRectangle, radius);
        using Pen penBorder = new Pen(borderColor, 1);

        graph.SmoothingMode = SmoothingMode.AntiAlias;
        control.Region = new Region(roundPath);

        graph.DrawPath(penBorder, roundPath);
    }

    private FormBoundsColors GetFormBoundsColors()
    {
        FormBoundsColors fbColor = new FormBoundsColors();
        using Bitmap bmp = new Bitmap(1, 1);
        using Graphics graph = Graphics.FromImage(bmp);

        Rectangle rectBmp = new Rectangle(0, 0, 1, 1);

        rectBmp.X = Bounds.X - 1;
        rectBmp.Y = Bounds.Y;
        graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
        fbColor.TopLeftColor = bmp.GetPixel(0, 0);

        rectBmp.X = Bounds.Right;
        rectBmp.Y = Bounds.Y;
        graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
        fbColor.TopRightColor = bmp.GetPixel(0, 0);

        rectBmp.X = Bounds.X;
        rectBmp.Y = Bounds.Bottom;
        graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
        fbColor.BottomLeftColor = bmp.GetPixel(0, 0);

        rectBmp.X = Bounds.Right;
        rectBmp.Y = Bounds.Bottom;
        graph.CopyFromScreen(rectBmp.Location, Point.Empty, rectBmp.Size);
        fbColor.BottomRightColor = bmp.GetPixel(0, 0);

        return fbColor;
    }

    #region Windows Form Designer generated code
    private void InitializeComponent()
    {
        contentPanel = new Panel();
        pictureBox1 = new PictureBox();
        errorProvider = new ErrorProvider(_components);
        contentPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        // 
        // contentPanel
        // 
        contentPanel.Controls.Add(pictureBox1);
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Location = new Point(0, 0);
        contentPanel.Name = "contentPanel";
        contentPanel.Size = new Size(325, 456);
        contentPanel.TabIndex = 0;
        // 
        // pictureBox1
        // 
        pictureBox1.Location = new Point(77, 12);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(139, 105);
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        // 
        // errorProvider
        // 
        errorProvider.ContainerControl = this;
        // 
        // AuthorizationForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(325, 456);
        Controls.Add(contentPanel);
        Name = "AuthorizationForm";
        Text = "Form1";
        Activated += Form_Activated;
        ResizeEnd += Form_ResizeEnd;
        SizeChanged += Form_SizeChanged;
        Paint += Form_Paint;
        MouseDown += Form_MouseDown;
        contentPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel contentPanel;
    private PictureBox pictureBox1;
    private ErrorProvider errorProvider;
    private System.ComponentModel.IContainer components;
}