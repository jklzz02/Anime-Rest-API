using System.Linq.Expressions;
using AnimeApi.Server.Core.Objects;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

/// <summary>
/// Defines a service for managing caching operations, such as storing, retrieving, and removing cached items.
/// </summary>
public interface ICachingService
{
    /// <summary>
    /// Gets or sets the default expiration timespan for cached items.
    /// This property is used to determine how long a cached item should remain in the cache
    /// when no explicit expiration is specified during the caching operation.
    /// </summary>
    TimeSpan DefaultExpiration { get; set; }

    /// <summary>
    /// The default size of cached items, used for cache size management.
    /// </summary>
    int DefaultItemSize { get; set; }

    /// <summary>
    /// Gets the current cache statistics.
    /// </summary>
    CacheStatus GetStatistics();

    /// <summary>
    /// Retrieves a cached item for the specified key or creates and caches a new item using the provided factory function
    /// if the key does not exist in the cache. 
    /// </summary>
    /// <typeparam name="T">The type of the cached item</typeparam>
    /// <param name="key">The unique key identifying the cached item.</param>
    /// <param name="factory">The function used to generate the item if it is not found in the cache.</param>
    /// <returns></returns>
    Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory);

    /// <summary>
    /// Retrieves a cached item for the specified key or creates and caches a new item using the provided factory function
    /// if the key does not exist in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="key">The unique key identifying the cached item.</param>
    /// <param name="factory">The function used to generate the item if it is not found in the cache.</param>
    /// <param name="size">The size of the cached item, used for cache size management.</param>
    /// <returns>The cached item if found, or the newly created item if it was not available in the cache.</returns>
    Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, int size);

    /// <summary>
    /// Retrieves a cached item for the specified key or creates and caches a new item using the provided factory function
    /// if the key does not exist in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="key">The unique key identifying the cached item.</param>
    /// <param name="factory">The function used to generate the item if it is not found in the cache.</param>
    /// <param name="size">The size of the cached item, used for cache size management.</param>
    /// <param name="expiration">The duration for which the cached item remains valid. If not specified, the default expiration time is used.</param>
    /// <returns>The cached item if found, or the newly created item if it was not available in the cache.</returns>
    Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, int size, TimeSpan expiration);

    /// <summary>
    /// Retrieves a cached item for the specified key or creates and caches a new item using the provided factory function
    /// if the key does not exist in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="factory">An <see cref="Expression"/> of the factory to call.</param>
    /// <returns>The cached item if found, or the newly created item if it was not available in the cache.</returns>
    /// <remarks>
    /// Automatically generates a unique key based on the provided factory expression.
    /// </remarks>
    Task<T?> GetOrCreateAsync<T>(Expression<Func<Task<T>>> factory);

    /// <summary>
    /// Retrieves a cached item for the specified key or creates and caches a new item using the provided factory function
    /// if the key does not exist in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="factory">An <see cref="Expression"/> of the factory to call.</param>
    /// <param name="size">The size of the cached item, used for cache size management.</param>
    /// <returns>The cached item if found, or the newly created item if it was not available in the cache.</returns>
    /// <remarks>
    /// Automatically generates a unique key based on the provided factory expression.
    /// </remarks>
    Task<T?> GetOrCreateAsync<T>(Expression<Func<Task<T>>> factory, int size);

    /// <summary>
    /// Retrieves a cached item for the specified key or creates and caches a new item using the provided factory function
    /// if the key does not exist in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the cached item.</typeparam>
    /// <param name="factory">An <see cref="Expression"/> of the factory to call.</param>
    /// <param name="size">The size of the cached item, used for cache size management.</param>
    /// <param name="expiration">The duration for which the cached item remains valid. If not specified, the default expiration time is used.</param>
    /// <returns>The cached item if found, or the newly created item if it was not available in the cache.</returns>
    /// <remarks>
    /// Automatically generates a unique key based on the provided factory expression.
    /// </remarks>
    Task<T?> GetOrCreateAsync<T>(Expression<Func<Task<T>>> factory, int size, TimeSpan expiration);

    /// <summary>
    /// Determines if a cached item exists for the specified key.
    /// </summary>
    /// <param name="key">The unique key identifying the cached item.</param>
    /// <returns>True if a cached item exists for the specified key; otherwise, false.</returns>
    public bool HasKey(object key);

    /// <summary>
    /// Removes the cached item associated with the specified key from the cache.
    /// </summary>
    /// <param name="key">The unique key identifying the cached item to be removed.</param>
    void Remove(object key);
}