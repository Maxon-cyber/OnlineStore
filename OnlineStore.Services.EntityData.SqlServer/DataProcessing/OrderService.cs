using OnlineStore.DataAccess.Repositories.SqlServer.Order;
using OnlineStore.Entities.Order;

namespace OnlineStore.Services.EntityData.SqlServer.DataProcessing;

public sealed class OrderService(OrderRepository orderRepository, FileLogger logger) : EntityService<OrderEntity>(orderRepository, logger)
{
    public async Task<OrderEntity?> GetOrderByIdAsync(Guid id)
    {
        OrderEntity? order = await GetByIdAsync("id", id, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetProductByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
        });

        return order;
    }

    public async Task<OrderEntity?> GetOrderByAsync(OrderEntity orderCondition)
    {
        OrderEntity? order = await GetByAsync(orderCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetOrderByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
        });

        return order;
    }

    public async Task<IEnumerable<OrderEntity>?> SelectOrdersAsync()
    {
        IEnumerable<OrderEntity>? orders = await SelectAsync(new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetAllOrders,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return orders;
    }

    public async Task<IEnumerable<OrderEntity>?> SelectOrdersByAsync(OrderEntity orderCondition)
    {
        IEnumerable<OrderEntity>? orders = await SelectByAsync(orderCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetAllOrdersByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return orders;
    }

    public async Task<bool> ChangeOrderAsync(TypeOfCommand typeOfCommand, OrderEntity order)
    {
        string command = typeOfCommand switch
        {
            TypeOfCommand.Insert => SqlServerStoredProcedureList.AddOrder,
            TypeOfCommand.Update => SqlServerStoredProcedureList.UpadateOrder,
            TypeOfCommand.Delete => SqlServerStoredProcedureList.DropOrder,
            _ => throw new NotImplementedException(),
        };

        object? result = await ChangeAsync(order, new QueryParameters()
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
                DbType = DbType.Int32,
                ParameterDirection = ParameterDirection.ReturnValue
            }
        });

        return Convert.ToBoolean(result);
    }

    public async Task<ImmutableDictionary<string, bool>> ChangeOrderAsync(TypeOfCommand typeOfCommand, IEnumerable<OrderEntity> orders)
    {
        string command = typeOfCommand switch
        {
            TypeOfCommand.Insert => SqlServerStoredProcedureList.AddOrder,
            TypeOfCommand.Update => SqlServerStoredProcedureList.UpadateOrder,
            TypeOfCommand.Delete => SqlServerStoredProcedureList.DropOrder,
            _ => throw new NotImplementedException(),
        };

        ImmutableDictionary<string, object?> result = await ChangeAsync(orders, new QueryParameters()
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
                DbType = DbType.Int32,
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