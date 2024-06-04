using OnlineStore.DataAccess.Relational.Models;
using OnlineStore.DataAccess.Repositories.Abstractions;
using OnlineStore.Entities.Product;

namespace OnlineStore.DataAccess.Repositories.SqlServer.Product;

public sealed class ProductRepository(SqlServerProvider<ProductEntity> sqlServer) : IRepository<ProductEntity>
{
    public string DbProviderName => "SqlServer";

    public async Task<DbResponse<ProductEntity>> GetByIdAsync(QueryParameters queryParameters, string? name, Guid? id, CancellationToken token)
       => await sqlServer.GetByIdAsync(queryParameters, name, id, token);

    public async Task<DbResponse<ProductEntity>> GetByIdsAsync(QueryParameters queryParameters, string? name, ICollection<Guid>? ids, CancellationToken token)
      => await sqlServer.GetByIdsAsync(queryParameters, name, ids, token);

    public async Task<DbResponse<ProductEntity>> GetByAsync(QueryParameters queryParameters, ProductEntity productCondition, CancellationToken token)
        => await sqlServer.GetByAsync(queryParameters, productCondition, token);

    public async Task<DbResponse<ProductEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => await sqlServer.SelectAsync(queryParameters, token);

    public async Task<DbResponse<ProductEntity>> SelectByAsync(QueryParameters queryParameters, ProductEntity productCondition, CancellationToken token)
        => await sqlServer.SelectByAsync(queryParameters, productCondition, token);

    public async Task<DbResponse<ProductEntity>> ChangeAsync(QueryParameters queryParameters, ProductEntity product, CancellationToken token)
        => await sqlServer.UpdateAsync(queryParameters, product, token);

    public async Task<IEnumerable<DbResponse<ProductEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<ProductEntity> products, CancellationToken token)
        => await sqlServer.UpdateAsync(queryParameters, products, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}