using OnlineStore.DataAccess.Relational.Models;
using OnlineStore.DataAccess.Repositories.Abstractions;
using OnlineStore.Entities.Order;

namespace OnlineStore.DataAccess.Repositories.SqlServer.Order;

public sealed class OrderRepository(SqlServerProvider<OrderEntity> sqlServer) : IRepository<OrderEntity>
{
    public string DbProviderName => "SqlServer";

    public async Task<DbResponse<OrderEntity>> GetByIdAsync(QueryParameters queryParameters, string? name, Guid? id, CancellationToken token)
       => await sqlServer.GetByIdAsync(queryParameters, "id", id, token);

    public async Task<DbResponse<OrderEntity>> GetByIdsAsync(QueryParameters queryParameters, string? name, ICollection<Guid>? ids, CancellationToken token)
       => await sqlServer.GetByIdsAsync(queryParameters, "id", ids, token);

    public async Task<DbResponse<OrderEntity>> GetByAsync(QueryParameters queryParameters, OrderEntity orderCondition, CancellationToken token)
        => await sqlServer.GetByAsync(queryParameters, orderCondition, token);

    public async Task<DbResponse<OrderEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => await sqlServer.SelectAsync(queryParameters, token);

    public async Task<DbResponse<OrderEntity>> SelectByAsync(QueryParameters queryParameters, OrderEntity orderCondition, CancellationToken token)
        => await sqlServer.SelectByAsync(queryParameters, orderCondition, token);

    public async Task<DbResponse<OrderEntity>> ChangeAsync(QueryParameters queryParameters, OrderEntity order, CancellationToken token)
        => await sqlServer.UpdateAsync(queryParameters, order, token);

    public async Task<IEnumerable<DbResponse<OrderEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<OrderEntity> orders, CancellationToken token)
        => await sqlServer.UpdateAsync(queryParameters, orders, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}