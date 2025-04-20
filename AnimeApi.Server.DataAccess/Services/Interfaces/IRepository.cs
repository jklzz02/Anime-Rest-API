namespace AnimeApi.Server.DataAccess.Services.Interfaces;

/// <summary>
/// Represents a generic repository interface for performing CRUD (Create, Read, Update, Delete) operations
/// on entities of a specified type.
/// </summary>
/// <typeparam name="T">The type of the entity the repository manages.</typeparam>
public interface IRepository<T> : IDisposable, IAsyncDisposable where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}