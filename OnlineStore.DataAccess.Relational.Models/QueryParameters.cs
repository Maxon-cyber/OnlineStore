using System.Data;

namespace OnlineStore.DataAccess.Relational.Models;

public sealed class QueryParameters()
{
    private string _commandText;

    public required string CommandText
    {
        get
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(_commandText);

            return _commandText;
        }
        init => _commandText = value;
    }

    public required CommandType CommandType { get; init; }

    public required bool TransactionManagementOnDbServer { get; init; }

    public Parameter? OutputParameter { get; init; }

    public Parameter? ReturnedValue { get; init; }
}

public sealed class Parameter()
{
    private int _size;
    private string _name;

    public required string Name
    {
        get
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(_name);

            return _name;
        }
        init => _name = value;
    }

    public required DbType DbType { get; init; }

    public int Size
    {
        get
        {
            if (_size == 0 && (DbType == DbType.AnsiString || DbType == DbType.Binary || DbType == DbType.String || DbType == DbType.StringFixedLength))
                throw new ArgumentException($"Для типа {DbType} должен быть указан размер");

            return _size;
        }
        init => _size = value;
    }

    public object? Value { get; init; }

    public required ParameterDirection ParameterDirection { get; init; }
}