using System.Collections.Concurrent;
using System.Linq.Expressions;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Services;

/// <summary>
/// Provides in-memory caching functionality with configurable expiration times.
/// Default cache expiration is set to 5 minutes if not specified otherwise.
/// </summary>
public class CachingService : ICachingService
{
    private readonly IMemoryCache _cache;
    private static readonly ConcurrentDictionary<Expression, Delegate> CompiledLambdas = new();
    private long _evictionCount = 0;

    /// <inheritdoc />
    public CacheStatus GetStatistics()
    {
        var stats = _cache.GetCurrentStatistics();
        
        if (stats is null)
        {
            return new CacheStatus();
        }

        return new CacheStatus
        {
            Hits = stats.TotalHits,
            Misses = stats.TotalMisses,
            EntriesCount = stats.CurrentEntryCount,
            EstimatedUnitSize = stats.CurrentEstimatedSize.GetValueOrDefault(),
            EvictionCount = Interlocked.Read(ref _evictionCount)
        };
    }

    /// <inheritdoc />
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(Constants.Cache.DefaultExpirationMinutes);

    /// <inheritdoc />
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
    public async Task<T?> GetOrCreateAsync<T>(Expression<Func<Task<T>>> factory)
        => await GetOrCreateAsync(AutoKey(factory), factory.Compile(), DefaultItemSize, DefaultExpiration);

    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(Expression<Func<Task<T>>> factory, int size)
        => await GetOrCreateAsync(AutoKey(factory), factory.Compile(), size, DefaultExpiration);

    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(Expression<Func<Task<T>>> factory, int size, TimeSpan expiration)
        => await GetOrCreateAsync(AutoKey(factory), factory.Compile(), size, expiration);

    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory)
        => await GetOrCreateAsync(key, factory, DefaultItemSize, DefaultExpiration);

    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, int size)
        => await GetOrCreateAsync(key, factory, size, DefaultExpiration);

    /// <inheritdoc />
    public async Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, int size, TimeSpan expiration)
    {
        return await _cache.GetOrCreateAsync(
            NormalizeKey(key),
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = expiration;
                entry.Size = size;
                
                entry.RegisterPostEvictionCallback((evictedKey, value, reason, state) =>
                {
                    if (reason == EvictionReason.Capacity)
                    {
                        Interlocked.Increment(ref _evictionCount);
                    }
                });

                return await factory();
            });
    }

    /// <inheritdoc />
    public bool HasKey(object key)
        => _cache.TryGetValue(NormalizeKey(key), out _);

    /// <inheritdoc />
    public void Remove(object key)
        => _cache.Remove(NormalizeKey(key));

    /// <summary>
    /// Returns a unique key based on the provided expression.
    /// </summary>
    /// <param name="expression">An <see cref="Expression"/>.</param>
    /// <returns>The automatic unique key.</returns>
    private string AutoKey(Expression expression)
    {
        var extracted = ExtractValue(expression);
        var keyData = new
        {
            Expression = expression.ToString(),
            Extracted = extracted
        };
        return NormalizeKey(keyData);
    }

    /// <summary>
    /// Normalizes the cache key by serializing and normalizing it.
    /// </summary>
    /// <param name="key">The key to be normalized.</param>
    /// <returns>The normalized key.</returns>
    private string NormalizeKey(object key)
        => SerializeKey(key).ToLowerNormalized();

    /// <summary>
    /// Normalizes the cache key by converting it to a string.
    /// </summary>
    /// <param name="key">The key to be serialized.</param>
    /// <returns>A serialized key.</returns>
    private string SerializeKey(object key)
    {
        return key switch
        {
            null => "null",
            string s => s,
            ValueType v => v.ToString()!,
            _ => JsonConvert.SerializeObject(key, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Include
            })
        };
    }

    /// <summary>
    /// Extracts the value represented by the given expression.
    /// </summary>
    /// <param name="expression">The <see cref="Expression"/> to extract the values from.</param>
    /// <returns>The extracted values</returns>
    private object? ExtractValue(Expression expression)
    {
        try
        {
            switch (expression)
            {
                case LambdaExpression lambdaExpr:
                    return ExtractValue(lambdaExpr.Body);

                case ConstantExpression constant:
                    return constant.Value;

                case MemberExpression member:
                    {
                        object? container = null;

                        if (member.Expression is not null)
                            container = ExtractValue(member.Expression);

                        if (member.Member is System.Reflection.FieldInfo field)
                            return field.IsStatic ? field.GetValue(null) : field.GetValue(container);

                        if (member.Member is System.Reflection.PropertyInfo property)
                            return property.GetMethod?.IsStatic == true
                                ? property.GetValue(null)
                                : property.GetValue(container);
                        break;
                    }

                case ConditionalExpression conditional:
                    {
                        var testValue = ExtractValue(conditional.Test);
                        if (testValue is bool condition)
                            return ExtractValue(condition ? conditional.IfTrue : conditional.IfFalse);

                        return new
                        {
                            Test = conditional.Test.ToString(),
                            IfTrue = conditional.IfTrue.ToString(),
                            IfFalse = conditional.IfFalse.ToString()
                        };
                    }

                case MethodCallExpression methodCall:
                    {
                        var target = ExtractValue(methodCall.Object!);
                        var args = methodCall.Arguments.Select(ExtractValue).ToArray();
                        return new
                        {
                            Target = target == null ? null : new { Type = target.GetType().FullName, Value = target },
                            Method = methodCall.Method.Name,
                            Args = args
                        };
                    }
            }

            if (typeof(Task).IsAssignableFrom(expression.Type))
                return expression.ToString();

            var lambda = CompiledLambdas.GetOrAdd(expression, expr => Expression.Lambda(expr).Compile());
            
            return lambda.DynamicInvoke();
        }
        catch
        {
            return expression.ToString();
        }
    }
}