
namespace AnimeApi.Server.Core.Abstractions.DataAccess.Specification;

/// <summary>
/// Represents a query specification interface used to apply filtering, sorting, and projection logic to a queryable data source.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
public interface IQuerySpec<TEntity> where TEntity : class
{
    /// <summary>
    /// Applies the specified query logic to the provided IQueryable object, including filtering, sorting, projection,
    /// and other modifications as defined in the implementation.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being queried.</typeparam>
    /// <param name="query">The queryable data source to which the specification logic will be applied.</param>
    /// <returns>A modified IQueryable instance representing the query with the applied specification logic.</returns>
    IQueryable<TEntity> Apply(IQueryable<TEntity> query);
}