using OnlineStore.MVP.ViewModels.MainWindow;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using System.Collections.ObjectModel;

namespace OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;

public interface IProductShowcaseView : IView
{
    event Func<Task<ReadOnlyCollection<ProductModel>?>> LoadProducts;

    event Func<ProductModel, Task> AddProduct;
}