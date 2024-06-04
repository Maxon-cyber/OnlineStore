using OnlineStore.MVP.Presenters.Common;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.Mainwindow;
using OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;
using OnlineStore.MVP.Views.Implementations.MainWindow.Sections;

namespace OnlineStore.MVP.Views.Implementations.MainWindow;

public sealed partial class MainWindowForm : Form, IMainWindowView
{
    private readonly ApplicationContext _context;

    private IUserAccountView _userAccount;
    private IProductShowcaseView _productShowcase;
    private IShoppingCartView _shoppingCart;

    public event Func<IUserAccountView> OpenUserAccount;
    public event Func<IProductShowcaseView> OpenProductShowcase;
    public event Func<IShoppingCartView> OpenShoppingCart;

    public MainWindowForm(ApplicationContext context)
    {
        _components = new System.ComponentModel.Container();

        InitializeComponent();

        _context = context;
    }

    void IView.Show()
    {
        _context.MainForm = this;

        _userAccount = OpenUserAccount.Invoke();
        _productShowcase = OpenProductShowcase.Invoke();
        _shoppingCart = OpenShoppingCart.Invoke();

        if (Settings.Default.RememberMe)
            Application.Run(_context);
        else
            Show();
    }

    private void BtnOpenUserAccount_Click(object sender, EventArgs e)
    {
        if (_userAccount == null)
            return;

        AddSection(_userAccount as AccountControl);
    }

    private void BtnOpenProductShowcase_Click(object sender, EventArgs e)
    {
        if (_productShowcase == null)
            return;

        AddSection(_productShowcase as ProductShowcaseControl);
    }

    private void BtnOpenShoppingCart_Click(object sender, EventArgs e)
    {
        if (_shoppingCart == null)
            return;

        AddSection(_shoppingCart as ShoppingCartControl);
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