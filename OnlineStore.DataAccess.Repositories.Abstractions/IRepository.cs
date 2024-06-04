using OnlineStore.DataAccess.Relational.Models;
using OnlineStore.Entities;

namespace OnlineStore.DataAccess.Repositories.Abstractions;

public interface IRepository<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : Entity
{
    string DbProviderName { get; }

    Task<DbResponse<TEntity>> GetByIdAsync(QueryParameters queryParameters, string? name, Guid? id, CancellationToken token);

    Task<DbResponse<TEntity>> GetByIdsAsync(QueryParameters queryParameters, string? name, ICollection<Guid>? ids, CancellationToken token);

    Task<DbResponse<TEntity>> GetByAsync(QueryParameters queryParameters, TEntity entityCondition, CancellationToken token);

    Task<DbResponse<TEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token);

    Task<DbResponse<TEntity>> SelectByAsync(QueryParameters queryParameters, TEntity userCondition, CancellationToken token);

    Task<DbResponse<TEntity>> ChangeAsync(QueryParameters queryParameters, TEntity user, CancellationToken token);

    Task<IEnumerable<DbResponse<TEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<TEntity> users, CancellationToken token);
}