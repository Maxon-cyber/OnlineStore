using OnlineStore.Core.Abstractions;
using OnlineStore.Entities.Order;
using OnlineStore.Entities.Product;
using OnlineStore.Entities.User;
using OnlineStore.MVP.Presenters.Abstractions.Contracts;
using OnlineStore.MVP.Presenters.Common;
using OnlineStore.MVP.ViewModels.MainWindow;
using OnlineStore.MVP.Views.Abstractions.MainInterface;
using OnlineStore.MVP.Views.Abstractions.Mainwindow.Sections;
using OnlineStore.Services.EntityData.SqlServer;
using OnlineStore.Services.EntityData.SqlServer.DataProcessing;
using OnlineStore.Services.Utilities.Caching.Abstractions;
using OnlineStore.Services.Utilities.Caching.InFile;
using OnlineStore.Services.Utilities.Caching.InMemory;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace OnlineStore.MVP.Presenters.MainWindow.ShoppingCart;

public sealed class ShoppingCartPresenter : Presenter<IShoppingCartView>
{
    private readonly MemoryCache _memoryCache;
    private readonly FileCache<OrderModel> _fileCache;
    private readonly OrderService _orderService;
    private readonly ProductService _productService;

    public ShoppingCartPresenter(IApplicationController controller, IShoppingCartView view, SqlServerService service, FileCache<OrderModel> cachedFile)
        : base(controller, view)
    {
        _orderService = service.Order;
        _productService = service.Product;

        _memoryCache = MemoryCache.Instance;
        _fileCache = cachedFile.SetFile("Order");

        _memoryCache.CacheChanged += Cache_Changed!;

        View.Order += Order;
        View.LoadOrders += LoadOrders;
    }

    private async Task<ReadOnlyCollection<OrderModel>?> LoadOrders()
    {
        if (!_fileCache.IsEmpty)
        {
            IImmutableDictionary<string, OrderModel> orderModels = await _fileCache.ReadAsync();

            if (orderModels.Count == 0)
            {
                View.ShowMessage("Нет заказов", "Ой...", MessageLevel.Error);
                return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(null);
            }

            return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(orderModels.Values.ToList().AsReadOnly());
        }

        IEnumerable<OrderEntity>? orders = await _orderService.SelectOrdersAsync();

        if (orders == null)
        {
            View.ShowMessage("Нет заказов", "Ой...", MessageLevel.Error);
            return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(null);
        }

        List<OrderModel> orderList = [];

        foreach (OrderEntity order in orders)
        {
            List<Guid> ids = order.Items.Select(o => o.ProductId).ToList();
            IEnumerable<ProductEntity>? products = await _productService.GetProductsByIdsAsync(ids);

            if (products == null)
            {
                View.ShowMessage("Не удалось загрузить продукты", "Ой...", MessageLevel.Error);
                return await Task.FromResult<ReadOnlyCollection<OrderModel>?>(null);
            }

            List<ProductModel> productModels = [];

            foreach (ProductEntity product in products)
                productModels.Add(new ProductModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Image = product.Image,
                    Price = product.Price,
                    Category = product.Category,
                    Quantity = product.Quantity
                });

            orderList.Add(new OrderModel
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Products = productModels,
                OrderDate = order.TimeCreated,
                Status = order.Status.ToString()
            });
        }

        return orderList.AsReadOnly();
    }

    private async Task Order(ICollection<ProductModel> products)
    {
        if (!Settings.Default.SingIn)
        {
            View.ShowMessage("Прежде чем оформить заказ, вы должны зарегистрироваться или войти в свой личный кабинет", "Ошибка", MessageLevel.Warning);
            return;
        }

        if (products.Count == 0)
        {
            View.ShowMessage("Что бы оформить заказ нужно добавить продукты", "Ошибка", MessageLevel.Error);
            return;
        }

        var groupedProducts = products.GroupBy(p => p.Id)
                                      .Select(g => new
                                      {
                                          ProductId = g.Key,
                                          NumberOfProducts = g.Count()
                                      }).ToList();

        OrderEntity orderEntity = new OrderEntity()
        {
            UserId = _memoryCache.Of<UserEntity>()!.FirstOrDefault()!.Id,
            Items = groupedProducts.Select(gp => new OrderItem()
            {
                ProductId = gp.ProductId,
                NumberOfProducts = gp.NumberOfProducts
            }).ToList(),
            TotalAmount = products.Sum(p => p.Price),
            DeliveryDate = DateTime.Now.AddDays(5),
            Status = Status.InProcessing
        };

        if (UserParameters.Internet.IsAvailable())
        {
            bool isAdded = await _orderService.ChangeOrderAsync(TypeOfCommand.Insert, orderEntity);

            if (isAdded)
                View.ShowMessage("Заказ успешно оформлен", "Успех", MessageLevel.Info);
            else
                View.ShowMessage("Заказ не оформлен", "Ой...", MessageLevel.Warning);
        }

        OrderModel orderModel = new OrderModel()
        {
            UserId = orderEntity.UserId,
            Products = [],
            TotalAmount = orderEntity.TotalAmount,
            OrderDate = orderEntity.TimeCreated,
            Status = orderEntity.Status.ToString()
        };

        foreach (ProductModel product in products)
            orderModel.Products.Add(new ProductModel()
            {
                Id = product.Id,
                Name = product.Name,
                Image = await _fileCache.SaveImageAsync((product.Image as byte[])!, true),
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity
            });

        await _fileCache.WriteAsync($"{orderModel.UserId}. Order Date: {orderModel.OrderDate}", orderModel);
    }

    private void Cache_Changed(object sender, CacheChangedEventArgs<string, object> cacheChanged)
    {
        switch (cacheChanged.ChangeType)
        {
            case CacheChangeType.Added:
                View.AddProduct(cacheChanged.Value as ProductModel);
                break;
            case CacheChangeType.Updated:
                break;
            case CacheChangeType.Removed:
                break;
        }
    }
}