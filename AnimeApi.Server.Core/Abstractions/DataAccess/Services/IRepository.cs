using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

/// <summary>
/// Defines the contract for a generic repository interface to perform operations on entities and their corresponding DTOs.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity associated with the repository.
/// </typeparam>
/// <typeparam name="TDto">
/// The type of the data transfer object (DTO) corresponding to the entity.
/// </typeparam>
public interface IRepository<TEntity, TDto> 
    where TEntity : class
    where TDto : class
{
    /// <summary>
    /// Retrieves all entities as a collection of data transfer objects (DTOs) from the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of DTOs representing all entities.</returns>
    Task<IEnumerable<TDto>> GetAllAsync();

    /// <summary>
    /// Retrieves entities matching the specified query specification as a collection of data transfer objects (DTOs).
    /// </summary>
    /// <param name="specification">The query specification defining the criteria for retrieving entities.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of DTOs for the entities matching the query specification.</returns>
    Task<IEnumerable<TDto>> FindAsync(IQuerySpec<TEntity> specification);

    /// <summary>
    ///  Retrieves a collection of entities projected to {TResult}.
    /// </summary>
    /// <param name="specification">The query specification defining the criteria for retrieving entities to project.</param>
    /// <typeparam name="TResult">The type to project to</typeparam>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of projected entities.</returns>
    Task<IEnumerable<TResult>> FindAsync<TResult>(IQuerySpec<TEntity> specification)
        where TResult : class, new();

    /// <summary>
    /// Retrieves the first entity that matches the provided specification projected to {TResult}.
    /// </summary>
    /// <param name="specification">The query specification defining the criteria for retrieving entity to project.</param>
    /// <typeparam name="TResult">The type to project to.</typeparam>
    /// <returns>A task representing the asynchronous operation. The task result contains the first matched projected entity, or null if no match is found.</returns>
    Task<TResult?> FindFirstOrDefaultAsync<TResult>(IQuerySpec<TEntity> specification)
        where TResult : class, new();
    
    /// <summary>
    /// Retrieves the first entity that matches the provided specification as a data transfer object (DTO) from the repository.
    /// </summary>
    /// <param name="specification">The query specification defining the criteria to match the entity.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the DTO representing the first matched entity, or null if no match is found.</returns>
    Task<TDto?> FindFirstOrDefaultAsync(IQuerySpec<TEntity> specification);
    
    /// <summary>
    /// Retrieves the total count of entities in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the total count of entities.</returns>
    Task<int> CountAsync();

    /// <summary>
    /// Counts the number of entities in the repository that match the specified query specification.
    /// </summary>
    /// <param name="specification">The query specification used to filter the entities.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the count of entities that satisfy the query specification.</returns>
    Task<int> CountAsync(IQuerySpec<TEntity> specification);

    /// <summary>
    /// Asynchronously adds a new entity to the repository and returns the result.
    /// </summary>
    /// <param name="dto">The data transfer object (DTO) representing the entity to be added.</param>
    /// <returns>A task representing the completion of the operation. The task result contains the result of the operation, including the added DTO or any errors encountered.</returns>
    Task<Result<TDto>> AddAsync(TDto dto);

    /// <summary>
    /// Adds a range of data transfer objects (DTOs) to the repository asynchronously.
    /// </summary>
    /// <param name="dto">The collection of DTOs to be added to the repository.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="Result{T}"/> object with the collection of added DTOs if the operation succeeds, or an error if it fails.</returns>
    Task<Result<IEnumerable<TDto>>> AddRangeAsync(IEnumerable<TDto> dto);

    /// <summary>
    /// Updates an existing entity in the repository with the provided data transfer object (DTO).
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated information of the entity.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <c>Result&lt;TDto&gt;</c> object indicating the outcome of the update operation and the updated entity.</returns>
    Task<Result<TDto>> UpdateAsync(TDto dto);

    /// <summary>
    /// Updates a collection of entities in the repository based on the given data transfer objects (DTOs).
    /// </summary>
    /// <param name="dto">The collection of data transfer objects (DTOs) containing the updated data for the entities.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="Result{T}"/> object with a collection of updated DTOs if successful, or error details if the operation fails.</returns>
    Task<Result<IEnumerable<TDto>>> UpdateRangeAsync(IEnumerable<TDto> dto);

    /// <summary>
    /// Deletes entities matching the specified query specification from the repository.
    /// </summary>
    /// <param name="specification">The query specification defining the entities to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the operation was successful.</returns>
    Task<bool> DeleteAsync(IQuerySpec<TEntity> specification);

    /// <summary>
    /// Deletes a range of entities from the repository that match the given specification.
    /// </summary>
    /// <param name="specification">The query specification defining the filter criteria for selecting entities to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean value indicating whether any entities were deleted.</returns>
    Task<bool> DeleteRangeAsync(IQuerySpec<TEntity> specification);

    /// <summary>
    /// Checks whether any entity satisfies the provided specification.
    /// </summary>
    /// <param name="specification">The query specification used to filter the entities.</param>
    /// <returns>A task representing the asynchronous operation. The task result is true if any entity satisfies the specification; otherwise, false.</returns>
    Task<bool> ExistsAsync(IQuerySpec<TEntity> specification);
}