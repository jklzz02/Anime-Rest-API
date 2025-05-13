using AnimeApi.Server.Business.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AnimeApi.Server.Business.Services;

public class CachingService : ICachingService
{
    private readonly IMemoryCache _cache;
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(30);

    public CachingService(IMemoryCache cache)
    {
        _cache = cache;
    }
    
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

    public bool HasKey(object key)
    {
        return _cache.TryGetValue(NormalizeKey(key), out _);
    }

    public void Remove(object key)
    {
        _cache.Remove(NormalizeKey(key));
    }
    
    private string NormalizeKey(object key) => key.ToString()!.ToLowerInvariant();
}