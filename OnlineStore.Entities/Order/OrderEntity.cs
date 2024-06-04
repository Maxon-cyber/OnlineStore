using OnlineStore.DataAccess.Relational.Attributes;
using System.Data;

namespace OnlineStore.Entities.Order;

public sealed class OrderEntity() : Entity
{
    [ColumnData("user_id", DbType.Guid)]
    [SqlParameter("user_id", DbType.Guid)]
    public Guid UserId { get; init; }

    [PointerToTable("OrderDetails")]
    [SqlParameter("order_details_table", DbType.Object)]
    public ICollection<OrderItem> Items { get; init; }

    [ColumnData("total_amount", DbType.Decimal)]
    [SqlParameter("total_amount", DbType.Decimal)]
    public decimal TotalAmount { get; init; }

    [ColumnData("delivery_date", DbType.DateTime2)]
    [SqlParameter("delivery_date", DbType.DateTime2)]
    public DateTime DeliveryDate { get; init; }

    [ColumnData("status", DbType.String)]
    [SqlParameter("status", DbType.String)]
    public Status Status { get; init; }

    [ColumnData("last_access_time", DbType.DateTime2)]
    [SqlParameter("last_access_time", DbType.DateTime2)]
    public DateTime LastAccessTime { get; init; }
}

public sealed class OrderItem()
{
    [ColumnData("product_id", DbType.Guid)]
    [SqlParameter("product_id", DbType.Guid)]
    public Guid ProductId { get; init; }

    [ColumnData("number_of_products", DbType.Int32)]
    [SqlParameter("number_of_products", DbType.Int32)]
    public int NumberOfProducts { get; init; }
}