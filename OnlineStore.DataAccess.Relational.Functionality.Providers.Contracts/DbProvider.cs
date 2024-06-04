using OnlineStore.DataAccess.Relational.Functionality.Tools.Mapper.Abstractions;
using OnlineStore.DataAccess.Relational.Models;
using System.Data.Common;

namespace OnlineStore.DataAccess.Relational.Functionality.Providers.Contracts;

public abstract class DbProvider<TParameter>(ConnectionParameters connectionParameters) : IDisposable, IAsyncDisposable
    where TParameter : class, new()
{
    protected readonly ConnectionParameters _connectionParameters = connectionParameters;

    protected DbConnection _dbConnection;
    protected DbCommand _dbCommand;

    public abstract string Prefix { get; }

    public abstract string Provider { get; }

    public abstract DbConnection DbConnection { get; }

    public abstract DbCommand DbCommand { get; }

    public abstract ValueTask<DbResponse<TParameter>> GetByIdAsync(QueryParameters queryParameters, string? name, Guid? id, CancellationToken token);

    public abstract ValueTask<DbResponse<TParameter>> GetByIdsAsync(QueryParameters queryParameters, string? name, ICollection<Guid>? ids, CancellationToken token);

    public abstract ValueTask<DbResponse<TParameter>> GetByAsync(QueryParameters queryParameters, TParameter parameterCondition, CancellationToken token);

    public abstract ValueTask<DbResponse<TParameter>> SelectAsync(QueryParameters queryParameters, CancellationToken token);

    public abstract ValueTask<DbResponse<TParameter>> SelectByAsync(QueryParameters queryParameters, TParameter parameterCondition, CancellationToken token);

    public abstract ValueTask<DbResponse<TParameter>> UpdateAsync(QueryParameters queryParameters, TParameter parameter, CancellationToken token);

    public abstract ValueTask<IEnumerable<DbResponse<TParameter>>> UpdateAsync(QueryParameters queryParameters, IEnumerable<TParameter> parameters, CancellationToken token);

    public abstract void Dispose();

    public abstract ValueTask DisposeAsync();
}