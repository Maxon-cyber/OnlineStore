using OnlineStore.MVP.ViewModels.MainWindow;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using System.Collections.ObjectModel;

namespace OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;

public interface IShoppingCartView : IView
{
    event Func<ICollection<ProductModel>, Task> Order;
    event Func<Task<ReadOnlyCollection<OrderModel>?>> LoadOrders;

    void AddProduct(ProductModel product);
}