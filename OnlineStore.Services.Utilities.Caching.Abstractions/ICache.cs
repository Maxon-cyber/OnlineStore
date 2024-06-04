using System.Collections.Immutable;

namespace OnlineStore.Services.Utilities.Caching.Abstractions;

public interface ICache<TKey, TValue>
    where TKey : notnull, IEquatable<TKey>
    where TValue : notnull
{
    event EventHandler<CacheChangedEventArgs<TKey, TValue>> CacheChanged;

    bool IsEmpty { get; }

    Task<IImmutableDictionary<string, TValue>> ReadAsync();

    Task<TValue?> ReadByKeyAsync(TKey key);

    Task WriteAsync(TKey key, TValue value);

    Task<bool> ContainsKeyAsync(TKey key);

    Task RemoveByAsync(TKey key);

    Task ClearAsync();
}

public sealed class CacheChangedEventArgs<TKey, TValue>(CacheChangeType cacheChangeType, TKey key, TValue value) : EventArgs
    where TKey : IEquatable<TKey>
    where TValue : notnull
{
    public CacheChangeType ChangeType { get; } = cacheChangeType;

    public TKey Key { get; } = key;

    public TValue Value { get; } = value;
}

public enum CacheChangeType
{
    Added,
    Updated,
    Removed
}