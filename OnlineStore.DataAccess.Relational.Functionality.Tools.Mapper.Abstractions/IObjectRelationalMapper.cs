using OnlineStore.DataAccess.Relational.Models;

namespace OnlineStore.DataAccess.Relational.Functionality.Tools.Mapper.Abstractions;

public interface IObjectRelationalMapper<TEntity> : IDisposable, IAsyncDisposable
    where TEntity : class, new()
{
    ValueTask<DbResponse<TEntity>> GetByIdAsync(QueryParameters query, string? name, Guid? id, CancellationToken token);

    ValueTask<DbResponse<TEntity>> GetByIdsAsync(QueryParameters query, string? name, ICollection<Guid>? ids, CancellationToken token);

    ValueTask<DbResponse<TEntity>> GetByAsync(QueryParameters query, TEntity entityCondition, CancellationToken token);

    ValueTask<DbResponse<TEntity>> SelectAsync(QueryParameters query, CancellationToken token);

    ValueTask<DbResponse<TEntity>> SelectByAsync(QueryParameters query, TEntity entityCondition, CancellationToken token);

    ValueTask<DbResponse<TEntity>> UpdateAsync(QueryParameters query, TEntity entity, CancellationToken token);

    ValueTask<IEnumerable<DbResponse<TEntity>>> UpdateAsync(QueryParameters query, IEnumerable<TEntity> entities, CancellationToken token);
}