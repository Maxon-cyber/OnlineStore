using OnlineStore.DataAccess.Repositories.SqlServer;
using OnlineStore.Services.EntityData.SqlServer.DataProcessing;

namespace OnlineStore.Services.EntityData.SqlServer;

public sealed class SqlServerService(SqlServerRepository sqlServerRepository, FileLogger logger) : IDisposable, IAsyncDisposable
{
    private readonly Lazy<UserService> _userService = new Lazy<UserService>(() => new UserService(sqlServerRepository.UserRepository, logger));
    private readonly Lazy<ProductService> _productService = new Lazy<ProductService>(() => new ProductService(sqlServerRepository.ProductRepository, logger));
    private readonly Lazy<OrderService> _orderService = new Lazy<OrderService>(() => new OrderService(sqlServerRepository.Order, logger));

    public UserService User => _userService.Value;

    public ProductService Product => _productService.Value;

    public OrderService Order => _orderService.Value;

    public void Dispose()
    {
        User?.Dispose();

        Product?.Dispose();

        Order?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (User != null)
            await User.DisposeAsync();

        if (Product != null)
            await Product.DisposeAsync();

        if (Order != null)
            await Order.DisposeAsync();
    }
}