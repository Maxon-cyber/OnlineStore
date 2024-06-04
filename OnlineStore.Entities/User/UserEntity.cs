using OnlineStore.DataAccess.Relational.Attributes;
using System.Data;

namespace OnlineStore.Entities.User;

public sealed class UserEntity() : Entity
{
    [ColumnData("name", DbType.String)]
    [SqlParameter("name", DbType.String)]
    public string Name { get; init; }

    [ColumnData("second_name", DbType.String)]
    [SqlParameter("second_name", DbType.String)]
    public string SecondName { get; init; }

    [ColumnData("patronymic", DbType.String)]
    [SqlParameter("patronymic", DbType.String)]
    public string Patronymic { get; init; }

    [ColumnData("gender", DbType.String)]
    [SqlParameter("gender", DbType.String)]
    public Gender Gender { get; init; }

    [ColumnData("age", DbType.Int32)]
    [SqlParameter("age", DbType.Int32)]
    public uint Age { get; init; }

    [ColumnData("login", DbType.String)]
    [SqlParameter("login", DbType.String)]
    public string Login { get; init; }

    [ColumnData("password", DbType.Binary)]
    [SqlParameter("password", DbType.Binary)]
    public byte[] Password { get; init; }

    [ColumnData("role", DbType.String)]
    [SqlParameter("role", DbType.String)]
    public Role Role { get; init; }

    [ColumnData("last_access_time", DbType.DateTime2)]
    [SqlParameter("last_access_time", DbType.DateTime2)]
    public DateTime LastAccessTime { get; init; }

    //[ColumnData("last_access_time", DbType.DateTime2)]
    //[SqlParameter("last_access_time", DbType.DateTime2)]
    //public ICollection<OrderEntity> LastAccessTime1 { get; init; }

    [PointerToTable("UserLocation")]
    public Location Location { get; init; }

    public override string ToString()
       => $"{SecondName} {Name} {Patronymic}";

    public override bool Equals(object? obj)
        => base.Equals(obj);

    public override int GetHashCode()
        => base.GetHashCode();
}

public sealed class Location()
{
    [ColumnData("house_number", DbType.String)]
    [SqlParameter("house_number", DbType.String)]
    public string HouseNumber { get; set; }

    [ColumnData("street", DbType.String)]
    [SqlParameter("street", DbType.String)]
    public string Street { get; set; }

    [ColumnData("city", DbType.String)]
    [SqlParameter("city", DbType.String)]
    public string City { get; init; }

    [ColumnData("region", DbType.String)]
    [SqlParameter("region", DbType.String)]
    public string Region { get; init; }

    [ColumnData("country", DbType.String)]
    [SqlParameter("country", DbType.String)]
    public string Country { get; init; }
}