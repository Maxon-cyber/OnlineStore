using OnlineStore.DataAccess.Relational.Attributes;
using System.Data;

namespace OnlineStore.Entities;

public abstract class Entity()
{
    [ColumnData("id", DbType.Guid)]
    [SqlParameter("id", DbType.Guid)]
    public Guid Id { get; set; }

    [ColumnData("time_created", DbType.DateTime2)]
    [SqlParameter("time_created", DbType.DateTime2)]
    public DateTime TimeCreated { get; set; }

    [ColumnData("last_update_time", DbType.DateTime2)]
    [SqlParameter("last_update_time", DbType.DateTime2)]
    public DateTime LastUpdateTime { get; set; }
}