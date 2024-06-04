using OnlineStore.DataAccess.Relational.Attributes;
using System.Data;

namespace OnlineStore.Entities.Product;

public sealed class ProductEntity() : Entity
{
    [ColumnData("name", DbType.String)]
    [SqlParameter("name", DbType.String)]
    public string Name { get; init; }

    [ColumnData("image", DbType.Binary)]
    [SqlParameter("image", DbType.Binary)]
    public byte[] Image { get; init; }

    [ColumnData("quantity", DbType.Int32)]
    [SqlParameter("quantity", DbType.Int32)]
    public int Quantity { get; init; }

    [ColumnData("category", DbType.String)]
    [SqlParameter("category", DbType.String)]
    public string Category { get; init; }

    [ColumnData("price", DbType.Decimal)]
    [SqlParameter("price", DbType.Decimal)]
    public decimal Price { get; init; }

    public override string ToString()
        => $"{Name}-{Price}";

    public override bool Equals(object? obj)
        => base.Equals(obj);

    public override int GetHashCode()
        => base.GetHashCode();
}