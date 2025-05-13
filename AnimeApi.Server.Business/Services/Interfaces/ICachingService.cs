namespace AnimeApi.Server.Business.Services.Interfaces;

public interface ICachingService
{
    TimeSpan DefaultExpiration { get; set; }
    Task<T?> GetOrCreateAsync<T>(object key, Func<Task<T>> factory, TimeSpan expiration = default);
    public bool HasKey(object key);
    void Remove(object key);
}