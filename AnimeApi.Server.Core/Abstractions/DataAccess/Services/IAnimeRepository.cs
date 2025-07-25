using System.Linq.Expressions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;
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
    /// Retrieves a paginated collection of anime entities filtered by their name.
    /// </summary>
    /// <param name="name">The name or partial name of the anime to filter by. Cannot be null or empty.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities that match the specified name for the given page and size.</returns>
    Task<PaginatedResult<Anime>> GetByNameAsync(string name, int page, int size = 100);

    /// <summary>
    /// Retrieves a paginated collection of anime entities filtered by their English name.
    /// </summary>
    /// <param name="englishName">The English name to search for. Cannot be null or empty.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities matching the specified English name for the given page and size.</returns>
    Task<PaginatedResult<Anime>> GetByEnglishNameAsync(string englishName, int page, int size = 100);

    /// <summary>
    /// Retrieves a collection of anime entities based on the specified source.
    /// </summary>
    /// <param name="source">The source to filter anime by. Must not be null or empty.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities that match the specified source.</returns>
    Task<PaginatedResult<Anime>> GetBySourceAsync(string source, int page, int size = 100);

    /// <summary>
    /// Retrieves a collection of anime entities filtered by their type.
    /// </summary>
    /// <param name="type">The type of anime to filter by. Must be a non-null and non-empty string.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities that match the specified type for the given page and size.</returns>
    Task<PaginatedResult<Anime>> GetByTypeAsync(string type, int page, int size = 100);

    /// <summary>
    /// Retrieves a collection of anime entities filtered by the specified score.
    /// </summary>
    /// <param name="score">The score to filter the anime by.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities with the specified score for the specified page and size.</returns>
    Task<PaginatedResult<Anime>> GetByScoreAsync(int score, int page, int size = 100);

    /// <summary>
    /// Retrieves a collection of anime entities released in the specified year, with optional pagination.
    /// </summary>
    /// <param name="year">The release year to filter the anime entities by.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities released in the specified year, for the given page and size.</returns>
    Task<PaginatedResult<Anime>> GetByReleaseYearAsync(int year, int page, int size = 100);

    /// <summary>
    /// Retrieves a paginated collection of anime entities filtered by the specified number of episodes.
    /// </summary>
    /// <param name="episodes">The exact number of episodes to filter the anime by.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities with the specified number of episodes for the given page and size.</returns>
    Task<PaginatedResult<Anime>> GetByEpisodesAsync(int episodes, int page, int size = 100);

    /// <summary>
    /// Retrieves a collection of anime entities associated with the specified licensor.
    /// </summary>
    /// <param name="licensorId">The unique identifier of the licensor.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities for the specified licensor and page parameters.</returns>
    Task<PaginatedResult<Anime>> GetByLicensorAsync(int licensorId, int page, int size = 100);

    /// <summary>
    /// Retrieves a paginated collection of anime entities associated with the specified producer.
    /// </summary>
    /// <param name="producerId">The unique identifier of the producer.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities associated with the specified producer for the provided page and size.</returns>
    Task<PaginatedResult<Anime>> GetByProducerAsync(int producerId, int page, int size = 100);

    /// <summary>
    /// Retrieves a collection of anime entities that belong to a specified genre.
    /// </summary>
    /// <param name="genreId">The identifier of the genre to filter by.</param>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <returns>A collection of anime entities associated with the specified genre for the given page and size.</returns>
    Task<PaginatedResult<Anime>> GetByGenreAsync(int genreId, int page, int size = 100);

    /// <summary>
    /// Retrieves a collection of anime entities that match the specified filter conditions, paginated by the given page and size.
    /// </summary>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="size">The number of items per page. Defaults to 100.</param>
    /// <param name="filters">A collection of filter expressions to apply to the query. Can be null for no filters.</param>
    /// <returns>A collection of anime entities that match the filter conditions for the specified page and size.</returns>
    Task<PaginatedResult<Anime>> GetByConditionAsync(
        int page,
        int size = 100,
        IEnumerable<Expression<Func<Anime, bool>>>? filters = null);

    /// <summary>
    /// Retrieves the first anime entity that satisfies the specified condition.
    /// </summary>
    /// <param name="condition">An expression that defines the condition to filter the anime entities.</param>
    /// <returns>The first anime entity that matches the condition or null if no match is found.</returns>
    Task<Anime?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition);

    /// <summary>
    /// Retrieves a limited collection of anime summaries.
    /// </summary>
    /// <param name="count">The number of anime summaries to retrieve. Must be greater than 0.</param>
    /// <returns>A collection of anime summary data transfer objects.</returns>
    Task<IEnumerable<AnimeSummary>> GetSummaryAsync(int count);
}