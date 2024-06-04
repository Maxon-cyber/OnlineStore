using OnlineStore.DataAccess.Relational.Models;
using OnlineStore.DataAccess.Repositories.Abstractions;
using OnlineStore.Entities.User;

namespace OnlineStore.DataAccess.Repositories.SqlServer.User;

public sealed class UserRepository(SqlServerProvider<UserEntity> sqlServer) : IRepository<UserEntity>
{
    public string DbProviderName => "SqlServer";

    public async Task<DbResponse<UserEntity>> GetByIdAsync(QueryParameters queryParameters, string? name, Guid? id, CancellationToken token)
        => await sqlServer.GetByIdAsync(queryParameters, "id", id, token);

    public async Task<DbResponse<UserEntity>> GetByIdsAsync(QueryParameters queryParameters, string? name, ICollection<Guid>? ids, CancellationToken token)
        => await sqlServer.GetByIdsAsync(queryParameters, "id", ids, token);

    public async Task<DbResponse<UserEntity>> GetByAsync(QueryParameters queryParameters, UserEntity userCondition, CancellationToken token)
        => await sqlServer.GetByAsync(queryParameters, userCondition, token);

    public async Task<DbResponse<UserEntity>> SelectAsync(QueryParameters queryParameters, CancellationToken token)
        => await sqlServer.SelectAsync(queryParameters, token);

    public async Task<DbResponse<UserEntity>> SelectByAsync(QueryParameters queryParameters, UserEntity userCondition, CancellationToken token)
        => await sqlServer.SelectByAsync(queryParameters, userCondition, token);

    public async Task<DbResponse<UserEntity>> ChangeAsync(QueryParameters queryParameters, UserEntity user, CancellationToken token)
        => await sqlServer.UpdateAsync(queryParameters, user, token);

    public async Task<IEnumerable<DbResponse<UserEntity>>> ChangeAsync(QueryParameters queryParameters, IEnumerable<UserEntity> users, CancellationToken token)
        => await sqlServer.UpdateAsync(queryParameters, users, token);

    public void Dispose()
        => sqlServer.Dispose();

    public async ValueTask DisposeAsync()
        => await sqlServer.DisposeAsync();
}