namespace OnlineStore.DataAccess.Relational.Models;

public sealed class DbResponse<TType>()
{
    private string _message = string.Empty;

    public Queue<TType> QueryResult { get; } = [];

    public Dictionary<object, object?> AdditionalData { get; } = new Dictionary<object, object?>();

    public string Message
    {
        get
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(_message);

            return _message;
        }
        set => _message = value;

    }

    public Exception? Error { get; set; } = null;

    public object? OutputValue { get; set; } = null;

    public object? ReturnedValue { get; set; } = null;
}