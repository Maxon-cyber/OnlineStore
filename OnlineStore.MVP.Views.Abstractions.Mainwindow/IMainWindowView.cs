using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;

namespace OnlineStore.MVP.Views.Abstractions.Mainwindow;

public interface IMainWindowView : IView
{
    event Func<IUserAccountView> OpenUserAccount;
    event Func<IProductShowcaseView> OpenProductShowcase;
    event Func<IShoppingCartView> OpenShoppingCart;
}