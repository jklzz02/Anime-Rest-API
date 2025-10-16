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
    /// Asynchronously retrieves all entities of the specified type.
    /// </summary>
    /// <returns>
    /// Returns a collection of entities of type <typeparamref name="TDto"/>.
    /// </returns>
    Task<IEnumerable<TDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves the first entity that matches the given specification.
    /// </summary>
    /// <param name="specification">The specification of type <typeparamref name="TEntity"/></param>
    /// <returns>Returns the retrieved entity.</returns>
    Task<TDto?> FindFirstOrDefaultAsync(ISpecification<TEntity> specification);

    /// <summary>
    /// Asynchronously retrieves all entities through the specified specification.
    /// </summary>
    /// <param name="specification">The specification of type <typeparamref name="TEntity"/></param>
    /// <returns>Returns a collection of entities based on the specification.</returns>
    Task<IEnumerable<TDto>> FindAsync(ISpecification<TEntity> specification);

    /// <summary>
    /// Asynchronously retrieves the count of entities that match the given specification.
    /// </summary>
    /// <param name="specification">The specification.</param>
    /// <returns>Returns the count of the entities that match the specification.</returns>
    Task<int> CountAsync(ISpecification<TEntity> specification);

    /// <summary>
    /// Asynchronously adds a new entity of the specified type to the repository.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    /// <returns>
    /// Returns the added entity of type <typeparamref name="TEntity"/> if the operation succeeds; otherwise, null.
    /// </returns>
    Task<Result<TDto>> AddAsync(TEntity entity);

    /// <summary>
    /// Asynchronously updates an existing entity of the specified type with new data.
    /// </summary>
    /// <param name="entity">The entity containing updated data.</param>
    /// <returns>
    /// Returns the updated entity of type <typeparamref name="TEntity"/> if the operation is successful; otherwise, null.
    /// </returns>
    Task<Result<TDto>> UpdateAsync(TEntity entity);

    /// <summary>
    /// Asynchronously deletes an entity of the specified type by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>
    /// Returns true if the entity was successfully deleted; otherwise, false.
    /// </returns>
    Task<bool> DeleteAsync(int id);
}