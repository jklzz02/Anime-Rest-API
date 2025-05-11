namespace AnimeApi.Server.Business.Services.Interfaces;

public interface ICachingService
{
    TimeSpan DefaultExpiration { get; }
    Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, TimeSpan expiration = default);
    void Remove(object key);
}