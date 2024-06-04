using System.Data;

namespace OnlineStore.DataAccess.Relational.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SqlParameterAttribute(string parameterName, DbType dbType) : Attribute
{
    public string ParameterName { get; } = parameterName;

    public DbType DbType { get; } = dbType;
}