namespace AnimeApi.Server.DataAccess.Services.Interfaces;

/// <summary>
/// Represents a generic repository interface for performing CRUD (Create, Read, Update, Delete) operations
/// on entities of a specified type.
/// </summary>
/// <typeparam name="TModel">The model type of the entity that the repository manages.</typeparam>
public interface IRepository<TModel> where TModel : class
{
    /// <summary>
    /// A property containing a collection of error messages. Each key-value pair represents
    /// an error description, where the key indicates the error name or type and the value
    /// provides a detailed message about the error.
    /// </summary>
    Dictionary<string, string> ErrorMessages { get; }

    /// <summary>
    /// Asynchronously retrieves an entity of the specified type by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>
    /// Returns the entity of type <typeparamref name="TModel"/> if found; otherwise, null.
    /// </returns>
    Task<TModel?> GetByIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves all entities of the specified type.
    /// </summary>
    /// <returns>
    /// Returns a collection of entities of type <typeparamref name="TModel"/>.
    /// </returns>
    Task<IEnumerable<TModel>> GetAllAsync();

    /// <summary>
    /// Asynchronously adds a new entity of the specified type to the repository.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    /// <returns>
    /// Returns the added entity of type <typeparamref name="TModel"/> if the operation succeeds; otherwise, null.
    /// </returns>
    Task<TModel?> AddAsync(TModel entity);

    /// <summary>
    /// Asynchronously updates an existing entity of the specified type with new data.
    /// </summary>
    /// <param name="entity">The entity containing updated data.</param>
    /// <returns>
    /// Returns the updated entity of type <typeparamref name="TModel"/> if the operation is successful; otherwise, null.
    /// </returns>
    Task<TModel?> UpdateAsync(TModel entity);

    /// <summary>
    /// Asynchronously deletes an entity of the specified type by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>
    /// Returns true if the entity was successfully deleted; otherwise, false.
    /// </returns>
    Task<bool> DeleteAsync(int id);
}