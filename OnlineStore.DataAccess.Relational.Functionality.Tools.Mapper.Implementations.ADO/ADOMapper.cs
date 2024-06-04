using OnlineStore.DataAccess.Relational.Functionality.Tools.Mapper.Abstractions;
using OnlineStore.DataAccess.Relational.Functionality.Tools.Mapper.Implementations.ADO.Exceptions;
using OnlineStore.DataAccess.Relational.Functionality.Tools.Mapper.Implementations.ADO.Extensions;
using OnlineStore.DataAccess.Relational.Models;
using System.Data.Common;

namespace OnlineStore.DataAccess.Relational.Functionality.Tools.Mapper.Implementations.ADO;

file enum ListOfSupportedDbProviders
{
    SqlServer = 0,
    Oracle = 1,
    MySQL = 2,
    PostgreSQL = 3,
    SQLite = 4,
    Firebird = 5,
    IBMDB2 = 6,
    Informix = 7,
    SQLServerCompactEdition = 8,
    MicrosoftAccess = 9,
    ODBC = 10,
    OLEDB = 11,
    DevartOracle = 12
}

public sealed class ADOMapper<TEntity> : IObjectRelationalMapper<TEntity>
    where TEntity : class, new()
{
    private readonly string _prefix;
    private readonly DbConnection _dbConnection;
    private readonly DbCommand _dbCommand;

    public ADOMapper(string provider, string prefix, DbConnection dbConnection, DbCommand dbCommand)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(provider));

        if (provider.Contains("Provider"))
            provider = provider.Replace("Provider", "");

        if (!Enum.TryParse(provider, true, out ListOfSupportedDbProviders _))
            throw new UnsupportedDbProviderException($"Провайдер {provider} не поддерживается технологией ADO.Net");

        _prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _dbCommand = dbCommand ?? throw new ArgumentNullException(nameof(dbCommand));
    }

    public async ValueTask<DbResponse<TEntity>> GetByIdAsync(QueryParameters query, string? name, Guid? id, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            _dbCommand.AddValue(id, _prefix, name, out DbParameter addedValue);

            _dbCommand.AddParameter(query.OutputParameter, _prefix, out DbParameter outputDbParameter);

            _dbCommand.AddParameter(query.ReturnedValue, _prefix, out DbParameter returnedDbParameter);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
            {
                List<TEntity> entities = await reader.MappingAsync<TEntity>(token);

                foreach (TEntity entity in entities)
                    response.QueryResult.Enqueue(entity);
            }

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async ValueTask<DbResponse<TEntity>> GetByIdsAsync(QueryParameters query, string? name, ICollection<Guid>? ids, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            if (ids != null)
                foreach (Guid id in ids)
                    _dbCommand.AddValue(id, _prefix, name, out DbParameter addedValue);

            _dbCommand.AddParameter(query.OutputParameter, _prefix, out DbParameter outputDbParameter);

            _dbCommand.AddParameter(query.ReturnedValue, _prefix, out DbParameter returnedDbParameter);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
            {
                List<TEntity> entities = await reader.MappingAsync<TEntity>(token);

                foreach (TEntity entity in entities)
                    response.QueryResult.Enqueue(entity);
            }

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter;
            response.ReturnedValue = returnedDbParameter;

            response.Message = "Запрос выполнен";

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async ValueTask<DbResponse<TEntity>> GetByAsync(QueryParameters query, TEntity condition, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            _dbCommand.AddParameter(query.OutputParameter, _prefix, out DbParameter outputDbParameter);
            _dbCommand.AddParameter(query.ReturnedValue, _prefix, out DbParameter returnedDbParameter);

            int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(condition, _prefix);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
            {
                List<TEntity> entities = await reader.MappingAsync<TEntity>(token);

                foreach (TEntity entity in entities)
                    response.QueryResult.Enqueue(entity);
            }

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async ValueTask<DbResponse<TEntity>> SelectAsync(QueryParameters query, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            _dbCommand.AddParameter(query.OutputParameter, _prefix, out DbParameter outputDbParameter);
            _dbCommand.AddParameter(query.ReturnedValue, _prefix, out DbParameter returnedDbParameter);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
            {
                List<TEntity> entities = await reader.MappingAsync<TEntity>(token);

                foreach (TEntity entity in entities)
                    response.QueryResult.Enqueue(entity);
            }

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async ValueTask<DbResponse<TEntity>> SelectByAsync(QueryParameters query, TEntity condition, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            _dbCommand.AddParameter(query.OutputParameter, _prefix, out DbParameter outputDbParameter);
            _dbCommand.AddParameter(query.ReturnedValue, _prefix, out DbParameter returnedDbParameter);

            int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(condition, _prefix);

            await using DbDataReader reader = await _dbCommand.ExecuteReaderAsync(token);

            if (reader.HasRows)
            {
                List<TEntity> entities = await reader.MappingAsync<TEntity>(token);

                foreach (TEntity entity in entities)
                    response.QueryResult.Enqueue(entity);
            }

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество полученных сущностей", response.QueryResult.Count);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async ValueTask<DbResponse<TEntity>> UpdateAsync(QueryParameters query, TEntity entity, CancellationToken token)
    {
        DbResponse<TEntity> response = new DbResponse<TEntity>();

        DbTransaction? dbTransaction = null;

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            _dbCommand.AddParameter(query.OutputParameter, _prefix, out DbParameter outputDbParameter);
            _dbCommand.AddParameter(query.ReturnedValue, _prefix, out DbParameter returnedDbParameter);

            int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(entity, _prefix);

            int countOfAddedRows = await _dbCommand.ExecuteNonQueryAsync(token);

            response.AdditionalData.TryAdd("Сущнотсть", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество переданных параметров для изменения сущности", countOfAddedValues);
            response.AdditionalData.TryAdd("Количество обновленных строк", countOfAddedRows);

            response.OutputValue = outputDbParameter?.Value;
            response.ReturnedValue = returnedDbParameter?.Value;

            response.Message = "Запрос выполнен";

            await dbTransaction.CommitTransactionAsync(token);
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return response;
    }

    public async ValueTask<IEnumerable<DbResponse<TEntity>>> UpdateAsync(QueryParameters query, IEnumerable<TEntity> entities, CancellationToken token)
    {
        List<DbResponse<TEntity>> responses = [];

        DbTransaction? dbTransaction = null;
        DbResponse<TEntity>? response = new DbResponse<TEntity>();

        try
        {
            await _dbConnection.OpenConnectionAsync(token);

            _dbCommand.CommandText = query.CommandText;
            _dbCommand.CommandType = query.CommandType;

            if (!query.TransactionManagementOnDbServer)
            {
                dbTransaction = await _dbConnection.BeginTransactionAsync(token);
                _dbCommand.Transaction = dbTransaction;
            }

            response.AdditionalData.TryAdd("Тип сущнотсти", typeof(TEntity).Name);
            response.AdditionalData.TryAdd("Количество переданных сущностей для изменения", entities.Count());

            foreach (TEntity entity in entities)
            {
                response = new DbResponse<TEntity>();

                _dbCommand.AddParameter(query.OutputParameter, _prefix, out DbParameter outputDbParameter);
                _dbCommand.AddParameter(query.ReturnedValue, _prefix, out DbParameter returnedDbParameter);

                int countOfAddedValues = await _dbCommand.AddEntityValuesAsync(entity, _prefix);

                int countOfAddedRows = await _dbCommand.ExecuteNonQueryAsync(token);

                response.AdditionalData.TryAdd("Количество переданных параметров для изменения сущности", countOfAddedValues);
                response.AdditionalData.TryAdd("Количество обновленных строк", countOfAddedRows);

                response.OutputValue = outputDbParameter?.Value;
                response.ReturnedValue = returnedDbParameter?.Value;

                response.Message = "Запрос выполнен";

                responses.Add(response);

                await dbTransaction.CommitTransactionAsync(token);
            }
        }
        catch (TimeoutException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (OperationCanceledException ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.Error = ex;
            response.Message = ex.Message;
        }
        finally
        {
            await _dbConnection.CloseConnectionAsync();

            _dbCommand.Parameters.Clear();

            await dbTransaction.DisposeAndRollbackTransactionAsync(token);
        }

        return responses;
    }

    public void Dispose()
    {
        GC.Collect();
        GC.SuppressFinalize(this);

        _dbConnection.Dispose();
        _dbCommand.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        GC.Collect();
        GC.SuppressFinalize(this);

        await _dbConnection.DisposeAsync();
        await _dbCommand.DisposeAsync();
    }
}