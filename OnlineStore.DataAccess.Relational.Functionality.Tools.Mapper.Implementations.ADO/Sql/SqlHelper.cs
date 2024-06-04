using System.Data;

namespace OnlineStore.DataAccess.Relational.Functionality.Tools.Mapper.Implementations.ADO.Sql;

internal static class SqlHelper
{
    internal static DbType ConvertToDbType(object? value) =>
        value?.GetType() switch
        {
            Type t when t == typeof(string) => DbType.String,
            Type t when t == typeof(byte) => DbType.Byte,
            Type t when t == typeof(short) => DbType.Int16,
            Type t when t == typeof(int) => DbType.Int32,
            Type t when t == typeof(long) => DbType.Int64,
            Type t when t == typeof(float) => DbType.Single,
            Type t when t == typeof(double) => DbType.Double,
            Type t when t == typeof(decimal) => DbType.Decimal,
            Type t when t == typeof(bool) => DbType.Boolean,
            Type t when t == typeof(DateTime) => DbType.DateTime,
            Type t when t == typeof(Guid) => DbType.Guid,
            Type t when t == typeof(object) => DbType.Object,
            _ => throw new ArgumentException($"Тип {value!.GetType().FullName} не поддерживается.")
        };

    internal static object ConvertToCLRType(object value, Type conversionType)
        => conversionType.IsEnum ? Enum.Parse(conversionType, (string)value)
                                 : Convert.ChangeType(value, conversionType);
}