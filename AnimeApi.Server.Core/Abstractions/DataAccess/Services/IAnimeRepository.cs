using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface IAnimeRepository : IRepository<Anime>
{
    /// <summary>
    /// Retrieves a paginated collection of all anime entities.
    /// </summary>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities for the specified page and size.</returns>
    Task<PaginatedResult<Anime>> GetAllAsync(int page, int size = 100);

    /// <summary>
    /// Retrieves a paginated collection of non-adult anime entities.
    /// </summary>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of non-mature anime entities for the specified page and size.</returns>
    Task<PaginatedResult<Anime>> GetAllNonAdultAsync(int page, int size);

    /// <summary>
    /// Retrieves a collection of anime entities by their unique identifiers.
    /// </summary>
    /// <param name="ids">An array of unique identifiers for the anime entities to retrieve.</param>
    /// <returns>A collection of anime entities matching the specified identifiers.</returns>
    Task<IEnumerable<Anime>> GetByIdsAsync(IEnumerable<int> ids);

    /// <summary>
    /// Retrieves the most recent anime entities added to the database.
    /// </summary>
    /// <param name="count">The number of most recent anime entities to retrieve. Must be greater than 0.</param>
    /// <returns>A collection containing the most recent anime entities, limited to the specified count.</returns>
    Task<IEnumerable<Anime>> GetMostRecentAsync(int count);

    /// <summary>
    ///  Retrieves paginated anime entities filtered by parameters.
    /// </summary>
    /// <param name="parameters">The parameters to filter the anime entities</param>
    /// <param name="page">The page number to retrieve. Must be grater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime that match the conditions set by the parameters.</returns>
    Task<PaginatedResult<Anime>> GetByParamsAsync(AnimeSearchParameters parameters, int page, int size = 100);

    /// <summary>
    /// Retrieves a limited collection of anime summaries.
    /// </summary>
    /// <param name="count">The number of anime summaries to retrieve. Must be greater than 0.</param>
    /// <returns>A collection of anime summary data transfer objects.</returns>
    Task<IEnumerable<AnimeSummary>> GetSummariesAsync(int count);
}