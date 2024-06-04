using OnlineStore.DataAccess.Repositories.SqlServer.User;
using OnlineStore.Entities.User;

namespace OnlineStore.Services.EntityData.SqlServer.DataProcessing;

public sealed class UserService(UserRepository userRepository, FileLogger logger) : EntityService<UserEntity>(userRepository, logger)
{
    public async Task<UserEntity?> GetUserByAsync(UserEntity userCondition)
    {
        UserEntity? user = await GetByAsync(userCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetUserByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
        });

        return user;
    }

    public async Task<IEnumerable<UserEntity>?> SelectUsersAsync()
    {
        IEnumerable<UserEntity>? users = await SelectAsync(new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetAllUsers,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return users;
    }

    public async Task<IEnumerable<UserEntity>?> SelectUsersByAsync(UserEntity userCondition)
    {
        IEnumerable<UserEntity>? users = await SelectByAsync(userCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetUsersByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return users;
    }

    public async Task<bool> ChangeUserAsync(TypeOfCommand typeOfCommand, UserEntity user)
    {
        string command = typeOfCommand switch
        {
            TypeOfCommand.Insert => SqlServerStoredProcedureList.AddUser,
            TypeOfCommand.Update => SqlServerStoredProcedureList.UpadateUser,
            TypeOfCommand.Delete => SqlServerStoredProcedureList.DropUser,
            _ => throw new NotImplementedException(),
        };

        object? result = await ChangeAsync(user, new QueryParameters()
        {
            CommandText = command,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
            OutputParameter = new Parameter()
            {
                Name = "@result",
                DbType = DbType.String,
                Size = -1,
                ParameterDirection = ParameterDirection.Output
            },
            ReturnedValue = new Parameter()
            {
                Name = "@returned_value",
                DbType = DbType.Boolean,
                ParameterDirection = ParameterDirection.ReturnValue
            }
        });

        return Convert.ToBoolean(result);
    }

    public async Task<ImmutableDictionary<string, bool>> ChangeUserAsync(TypeOfCommand typeOfCommand, IEnumerable<UserEntity> users)
    {
        string command = typeOfCommand switch
        {
            TypeOfCommand.Insert => SqlServerStoredProcedureList.AddUser,
            TypeOfCommand.Update => SqlServerStoredProcedureList.AddUser,
            TypeOfCommand.Delete => SqlServerStoredProcedureList.DropUser,
            _ => throw new NotImplementedException(),
        };

        ImmutableDictionary<string, object?> result = await ChangeAsync(users, new QueryParameters()
        {
            CommandText = command,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
            OutputParameter = new Parameter()
            {
                Name = "@result",
                Size = -1,
                DbType = DbType.Int32,
                ParameterDirection = ParameterDirection.Output
            },
            ReturnedValue = new Parameter()
            {
                Name = "@returned_value",
                DbType = DbType.Boolean,
                ParameterDirection = ParameterDirection.ReturnValue
            }
        });

        Dictionary<string, bool> boolDictionary = result.ToDictionary(kvp => kvp.Key, kvp => Convert.ToBoolean(kvp.Value));

        ImmutableDictionary<string, bool> immutableBoolDictionary = boolDictionary.ToImmutableDictionary();

        return immutableBoolDictionary;
    }

    public new void Dispose()
        => base.Dispose();

    public new async ValueTask DisposeAsync()
        => await base.DisposeAsync();
}