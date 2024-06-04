using OnlineStore.DataAccess.Repositories.SqlServer.Product;
using OnlineStore.Entities.Product;

namespace OnlineStore.Services.EntityData.SqlServer.DataProcessing;

public sealed class ProductService(ProductRepository repository, FileLogger logger) : EntityService<ProductEntity>(repository, logger)
{
    public async Task<ProductEntity?> GetProductByIdAsync(Guid id)
    {
        ProductEntity? product = await GetByIdAsync("id", id, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetProductByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
        });

        return product;
    }

    public async Task<IEnumerable<ProductEntity>?> GetProductsByIdsAsync(ICollection<Guid> ids)
    {
        string idsString = string.Join(",", ids.Select(id => $"'{id}'"));

        IEnumerable<ProductEntity>? product = await GetByIdsAsync(null, null, new QueryParameters()
        {
            CommandText = $"select * from Products where id in ({idsString})",
            CommandType = CommandType.Text,
            TransactionManagementOnDbServer = true,
        });

        return product;
    }

    public async Task<ProductEntity?> GetProductByAsync(ProductEntity productCondition)
    {
        ProductEntity? product = await GetByAsync(productCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetProductByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
        });

        return product;
    }

    public async Task<IEnumerable<ProductEntity>?> SelectProductsAsync()
    {
        IEnumerable<ProductEntity>? products = await SelectAsync(new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetAllProducts,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true
        });

        return products;
    }

    public async Task<IEnumerable<ProductEntity>?> SelectProductsByAsync(ProductEntity productCondition)
    {
        IEnumerable<ProductEntity>? products = await SelectByAsync(productCondition, new QueryParameters()
        {
            CommandText = SqlServerStoredProcedureList.GetAllProductsByCondition,
            CommandType = CommandType.StoredProcedure,
            TransactionManagementOnDbServer = true,
        });

        return products;
    }

    public async Task<bool> ChangeProductAsync(TypeOfCommand typeOfCommand, ProductEntity product)
    {
        string command = typeOfCommand switch
        {
            TypeOfCommand.Insert => SqlServerStoredProcedureList.AddProduct,
            TypeOfCommand.Update => SqlServerStoredProcedureList.UpadateProduct,
            TypeOfCommand.Delete => SqlServerStoredProcedureList.DropProduct,
            _ => throw new NotImplementedException(),
        };

        object? result = await ChangeAsync(product, new QueryParameters()
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

    public async Task<ImmutableDictionary<string, bool>> ChangeProductAsync(TypeOfCommand typeOfCommand, IEnumerable<ProductEntity> products)
    {
        string command = typeOfCommand switch
        {
            TypeOfCommand.Insert => SqlServerStoredProcedureList.AddProduct,
            TypeOfCommand.Update => SqlServerStoredProcedureList.UpadateProduct,
            TypeOfCommand.Delete => SqlServerStoredProcedureList.DropProduct,
            _ => throw new NotImplementedException(),
        };

        ImmutableDictionary<string, object?> result = await ChangeAsync(products, new QueryParameters()
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