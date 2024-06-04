using OnlineStore.Services.Utilities.Caching.Abstractions;
using System.Collections.Immutable;

namespace OnlineStore.Services.Utilities.Caching.InMemory;

file static class CacheStorage
{
    public static Dictionary<string, object> Cache { get; }

    static CacheStorage()
        => Cache = new Dictionary<string, object>();
}

public sealed class MemoryCache : ICache<string, object>
{
    private uint _subscriberCount;

    private static readonly Lazy<MemoryCache> _lazyInstance = new Lazy<MemoryCache>(() => new MemoryCache());
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 20);

    private event EventHandler<CacheChangedEventArgs<string, object>> Cache_Changed;

    public static MemoryCache Instance => _lazyInstance.Value;

    private MemoryCache() { }

    public event EventHandler<CacheChangedEventArgs<string, object>>? CacheChanged
    {
        add
        {
            Cache_Changed += value;
            _subscriberCount++;
        }
        remove
        {
            if (_subscriberCount == 0)
                return;

            Cache_Changed -= value;
            _subscriberCount--;
        }
    }

    public bool IsEmpty => CacheStorage.Cache.Count == 0;

    public IEnumerable<TType>? Of<TType>()
        where TType : notnull
    {
        IEnumerable<KeyValuePair<string, object>>? findDataOfType = CacheStorage.Cache.Where(kvp => kvp.Value is TType);

        IEnumerable<TType>? result = findDataOfType?.Select(kvp => (TType)kvp.Value);

        return result;
    }

    public IEnumerable<TType>? Of<TType>(Func<TType, bool> func)
        where TType : notnull
    {
        IEnumerable<KeyValuePair<string, object>>? findDataOfType = CacheStorage.Cache.Where(kvp => kvp.Value is TType);

        IEnumerable<TType>? result = findDataOfType?.Select(kvp => (TType)kvp.Value).Where(func);

        return result;
    }

    public async Task ClearAsync()
    {
        await _semaphore.WaitAsync();

        CacheStorage.Cache.Clear();

        _semaphore.Release();
    }

    public async Task<bool> ContainsKeyAsync(string key)
    {
        await _semaphore.WaitAsync();

        bool isContains = await Task.FromResult(CacheStorage.Cache.ContainsKey(key));

        _semaphore.Release();

        return isContains;
    }

    public async Task<IImmutableDictionary<string, object>> ReadAsync()
    {
        await _semaphore.WaitAsync();

        ImmutableDictionary<string, object> result = CacheStorage.Cache
                                                                .ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value);

        if (result.Count == 0)
            return await Task.FromResult<IImmutableDictionary<string, object>>(ImmutableDictionary<string, object>.Empty);

        _semaphore.Release();

        return result;
    }

    public async Task<object?> ReadByKeyAsync(string key)
    {
        await _semaphore.WaitAsync();

        if (CacheStorage.Cache.TryGetValue(key, out object? value))
            return await Task.FromResult(value);

        _semaphore.Release();

        return await Task.FromResult(default(object));
    }

    public async Task RemoveByAsync(string key)
    {
        await _semaphore.WaitAsync();

        if (CacheStorage.Cache.TryGetValue(key, out object? value))
        {
            CacheStorage.Cache.Remove(key);
            OnCacheChanged(new CacheChangedEventArgs<string, object>(CacheChangeType.Removed, key, value));
        }

        _semaphore.Release();
    }

    public async Task WriteAsync(string key, object value)
    {
        await _semaphore.WaitAsync();

        CacheStorage.Cache[key] = value;

        OnCacheChanged(new CacheChangedEventArgs<string, object>(CacheChangeType.Added, key, value));

        _semaphore.Release();
    }

    private void OnCacheChanged(CacheChangedEventArgs<string, object> e)
        => Cache_Changed?.Invoke(this, e);
}