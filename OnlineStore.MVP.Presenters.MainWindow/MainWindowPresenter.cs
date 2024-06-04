using OnlineStore.Core.Abstractions;
using OnlineStore.MVP.Presenters.Abstractions.Contracts;
using OnlineStore.MVP.Presenters.MainWindow.Account;
using OnlineStore.MVP.Presenters.MainWindow.ProductShowcase;
using OnlineStore.MVP.Presenters.MainWindow.ShoppingCart;
using OnlineStore.MVP.Views.Abstractions.Mainwindow;
using OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;

namespace OnlineStore.MVP.Presenters.MainWindow;

public sealed class MainWindowPresenter : Presenter<IMainWindowView>
{
    public MainWindowPresenter(IApplicationController controller, IMainWindowView view)
        : base(controller, view)
    {
        View.OpenUserAccount += OpenUserAccount;
        View.OpenProductShowcase += OpenProductShowcase;
        View.OpenShoppingCart += OpenShoppingCart;
    }

    private IUserAccountView OpenUserAccount()
       => Controller.Run<IUserAccountView, UserAccountPresenter>();

    private IProductShowcaseView OpenProductShowcase()
       => Controller.Run<IProductShowcaseView, ProductShowcasePresenter>();

    private IShoppingCartView OpenShoppingCart()
       => Controller.Run<IShoppingCartView, ShoppingCartPresenter>();
}