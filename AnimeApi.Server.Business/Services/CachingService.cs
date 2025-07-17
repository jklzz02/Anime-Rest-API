using AnimeApi.Server.Core;
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

    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(Constants.Cache.DefaultExpirationMinutes);

    public int DefaultItemSize { get; set; } = Constants.Cache.DefaultCachedItemSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachingService"/> class.
    /// </summary>
    /// <param name="cache">The memory cache implementation to be used for storing cached items.</param>
    public CachingService(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachingService"/> class with a specified default expiration time.
    /// </summary>
    /// <param name="cache">The memory cache implementation to be used for storing cached items.</param>
    /// <param name="defaultExpiration">Default expiration time for cached items.</param>
    public CachingService(IMemoryCache cache, TimeSpan defaultExpiration)
    {
        _cache = cache;
        DefaultExpiration = defaultExpiration;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CachingService"/> class with a specified default expiration time.
    /// </summary>
    /// <param name="cache">The memory cache implementation to be used for storing cached items.</param>
    /// <param name="defaultExpiration">Default expiration time for cached items.</param>
    /// <param name="defaultItemSize">Default size for cached items, used for cache size management.</param>
    public CachingService(IMemoryCache cache, TimeSpan defaultExpiration, int defaultItemSize)
    {
        _cache = cache;
        DefaultExpiration = defaultExpiration;
        DefaultItemSize = defaultItemSize;
    }

    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory)
    {
        return await GetOrCreateAsync(key, factory, DefaultItemSize, DefaultExpiration);
    }

    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, int size)
    {
        return await GetOrCreateAsync(key, factory, DefaultItemSize, DefaultExpiration);
    }

    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, int size, TimeSpan expiration)
    {
        if (_cache.TryGetValue(NormalizeKey(key), out T? value))
        {
            return value;
        }

        value = await factory();
        if (value is not null)
        {
            _cache
                .Set(
                    NormalizeKey(key),
                    value,
                    new MemoryCacheEntryOptions 
                    {
                        AbsoluteExpirationRelativeToNow = expiration,
                        Size = size
                    });
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