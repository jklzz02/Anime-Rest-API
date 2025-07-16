using AnimeApi.Server.Core.Abstractions.Business.Services;
using Microsoft.Extensions.Caching.Memory;

namespace AnimeApi.Server.Business.Services;

/// <summary>
/// Provides in-memory caching functionality with configurable expiration times.
/// Default cache expiration is set to 5 minutes if not specified otherwise.
/// </summary>
public class CachingService : ICachingService
{
    private readonly IMemoryCache _cache;
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CachingService"/> class.
    /// </summary>
    /// <param name="cache">The memory cache implementation to be used for storing cached items.</param>
    public CachingService(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, TimeSpan expiration = default)
    {
        if (_cache.TryGetValue(NormalizeKey(key), out T? value))
        {
            return value;
        }

        value = await factory();
        if (value is not null)
        {
            _cache.Set(NormalizeKey(key), value, expiration == TimeSpan.Zero ? DefaultExpiration : expiration);
        }

        return value;
    }

    /// <inheritdoc />
    public bool HasKey(object key)
    {
        return _cache.TryGetValue(NormalizeKey(key), out _);
    }

    /// <inheritdoc />
    public void Remove(object key)
    {
        _cache.Remove(NormalizeKey(key));
    }
    
    private string NormalizeKey(object key) => key.ToString()!.ToLowerInvariant();
}