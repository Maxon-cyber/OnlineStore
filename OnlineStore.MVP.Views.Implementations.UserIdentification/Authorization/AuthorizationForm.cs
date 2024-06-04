using OnlineStore.MVP.ViewModels.Common.Validation;
using OnlineStore.MVP.ViewModels.UserIdentification;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.UserIdentification;
using OnlineStore.MVP.Views.Implementations.UserIdentification.Registration;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace OnlineStore.MVP.Views.Implementations.UserIdentification.Authorization;

public sealed partial class AuthorizationForm : Form, IAuthorizationView
{
    private bool _isRunable = false;
    private readonly ApplicationContext _context;

    public event Func<AuthorizationViewModel, bool, Task> Authorization;
    public event Func<IRegistrationView> Registration;

    public AuthorizationForm(ApplicationContext context)
    {
        _components = new System.ComponentModel.Container();

        InitializeComponent();

        FormBorderStyle = FormBorderStyle.None;
        Padding = new Padding(BORDER_SIZE);

        _context = context;
    }

    void IView.Show()
    {
        if (!_isRunable)
        {
            _isRunable = true;

            _context.MainForm = this;
            Application.Run(_context);
        }
    }

    private async void BtnLogin_Click(object sender, EventArgs e)
    {
        //loginButton.Enabled = false;

        errorProvider.Clear();

        IEnumerable<TextBox> emptyTextBoxes = contentPanel.Controls.OfType<TextBox>()
                                                                   .Where(tb => string.IsNullOrWhiteSpace(tb.Text));

        if (emptyTextBoxes.Any())
        {
            foreach (TextBox textBox in emptyTextBoxes)
                errorProvider.SetError(textBox, $"¬ведите значение {textBox.PlaceholderText}");

            return;
        }

        //AuthorizationViewModel user = new AuthorizationViewModel()
        //{
        //    Login = loginTextBox.Text,
        //    Password = passwordTextBox.Text
        //};

        //IList<ValidationResult> validationResults = ModelValidator.Validate(user);

        //if (validationResults.Count > 0)
        //{
        //    foreach (ValidationResult validationResult in validationResults)
        //    {
        //        string errorTextBoxName = $"{validationResult.MemberNames.FirstOrDefault()!.ToLower()}TextBox";
        //        errorProvider.SetError(contentPanel.Controls[errorTextBoxName]!, validationResult.ErrorMessage);
        //    }

        //    return;
        //}

        //await Authorization.Invoke(user, rememberMeCheckBox.Checked);

        //loginButton.Enabled = true;
    }

    private void BtnRegistration_Click(object sender, EventArgs e)
    {
        IRegistrationView registration = Registration.Invoke();

        if (registration == null)
            return;

        RegistrationControl registrationControl = (registration as RegistrationControl)!;
        registrationControl.BorderStyle = BorderStyle.None;
        registrationControl.Dock = DockStyle.Fill;

        contentPanel.Controls.Add(registrationControl);
        contentPanel.Tag = registrationControl;

        registrationControl.BringToFront();
    }

    private void ContentPanel_Paint(object sender, PaintEventArgs e)
        => ControlRegionAndBorder(contentPanel, BORDER_RADIUS - (BORDER_SIZE / 2), e.Graphics, _borderColor);

    private void Form_ResizeEnd(object sender, EventArgs e)
        => Invalidate();

    private void Form_SizeChanged(object sender, EventArgs e)
        => Invalidate();

    private void Form_Activated(object sender, EventArgs e)
        => Invalidate();

    private void ContentPanel_MouseDown(object sender, MouseEventArgs e)
    {
        ReleaseCapture();
        SendMessage(Handle, 0x112, 0xf012, 0);
    }

    private void Form_MouseDown(object sender, MouseEventArgs e)
    {
        ReleaseCapture();
        SendMessage(Handle, 0x112, 0xf012, 0);
    }

    private void Form_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle rectForm = ClientRectangle;

        int mWidht = rectForm.Width / 2;
        int mHeight = rectForm.Height / 2;
        FormBoundsColors fbColors = GetFormBoundsColors();

        DrawPath(rectForm, e.Graphics, fbColors.TopLeftColor);
        Rectangle rectTopRight = new Rectangle(mWidht, rectForm.Y, mWidht, mHeight);

        DrawPath(rectTopRight, e.Graphics, fbColors.TopRightColor);
        Rectangle rectBottomLeft = new Rectangle(rectForm.X, rectForm.X + mHeight, mWidht, mHeight);

        DrawPath(rectBottomLeft, e.Graphics, fbColors.BottomLeftColor);
        Rectangle rectBottomRight = new Rectangle(mWidht, rectForm.Y + mHeight, mWidht, mHeight);

        DrawPath(rectBottomRight, e.Graphics, fbColors.BottomRightColor);
        FormRegionAndBorder(this, BORDER_RADIUS, e.Graphics, _borderColor, BORDER_SIZE);
    }

    private void contentPanel_Paint_1(object sender, PaintEventArgs e)
    {

    }

    void IView.ShowMessage(string message, string caption, MessageLevel level)
         => MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, level switch
         {
             MessageLevel.Info => MessageBoxIcon.Information,
             MessageLevel.Warning => MessageBoxIcon.Warning,
             MessageLevel.Error => MessageBoxIcon.Error,
             _ => MessageBoxIcon.None,
         });

    void IView.Close()
        => Close();
}