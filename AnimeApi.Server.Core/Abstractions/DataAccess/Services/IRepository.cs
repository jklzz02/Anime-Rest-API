using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

/// <summary>
/// Represents a generic repository interface for performing CRUD (Create, Read, Update, Delete) operations
/// on entities of a specified type.
/// </summary>
/// <typeparam name="TEntity">The model type of the entity that the repository manages.</typeparam>
public interface IRepository<TEntity, TDto> 
    where TEntity : class
    where TDto : class
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TDto>> GetAllAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    Task<TDto?> FindFirstOrDefaultAsync(IQuerySpec<TEntity> specification);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    Task<IEnumerable<TDto>> FindAsync(IQuerySpec<TEntity> specification);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<int> CountAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    Task<int> CountAsync(IQuerySpec<TEntity> specification);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Result<TDto>> AddAsync(TEntity entity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Result<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TEntity> entity);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Result<TDto>> UpdateAsync(TEntity entity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(IQuerySpec<TEntity> specification);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    Task<bool> DeleteRangeAsync(IQuerySpec<TEntity> specification);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="specification"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(IQuerySpec<TEntity> specification);
}