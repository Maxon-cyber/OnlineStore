using OnlineStore.DataAccess.Relational.Models;
using OnlineStore.DataAccess.Repositories.SqlServer.Order;
using OnlineStore.DataAccess.Repositories.SqlServer.Product;
using OnlineStore.DataAccess.Repositories.SqlServer.User;
using OnlineStore.Entities.Order;
using OnlineStore.Entities.Product;
using OnlineStore.Entities.User;

namespace OnlineStore.DataAccess.Repositories.SqlServer;

public sealed class SqlServerRepository
{
    private readonly Lazy<UserRepository> _userRepository;
    private readonly Lazy<ProductRepository> _productRepository;
    private readonly Lazy<OrderRepository> _orderRepository;

    public UserRepository UserRepository => _userRepository.Value;

    public ProductRepository ProductRepository => _productRepository.Value;

    public OrderRepository Order => _orderRepository.Value;

    public SqlServerRepository(ConnectionParameters connectionParameters)
    {
        if (!string.Equals(connectionParameters.Provider, "SqlServer", StringComparison.CurrentCultureIgnoreCase))
            throw new ArgumentException();

        _userRepository = new Lazy<UserRepository>(() => new UserRepository(new SqlServerProvider<UserEntity>(connectionParameters)));
        _productRepository = new Lazy<ProductRepository>(() => new ProductRepository(new SqlServerProvider<ProductEntity>(connectionParameters)));
        _orderRepository = new Lazy<OrderRepository>(() => new OrderRepository(new SqlServerProvider<OrderEntity>(connectionParameters)));
    }
}