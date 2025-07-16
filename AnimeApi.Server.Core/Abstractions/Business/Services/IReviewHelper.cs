using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

/// <summary>
/// Provides functionality to manage <see cref="ReviewDto"/> data in the application.
/// </summary>
public interface IReviewHelper
{
    /// <summary>
    /// Provides a dictionary containing error messages associated with various
    /// operations or validations related to review management.
    /// </summary>
    public Dictionary<string, string> ErrorMessages { get; }

    /// <summary>
    /// Retrieves a <see cref="ReviewDto"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review to retrieve.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains a <see cref="ReviewDto"/> if found, or null if not.</return>
    Task<ReviewDto?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of <see cref="ReviewDto"/> associated with a specific user identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose reviews are to be retrieved.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ReviewDto"/> associated with the specified user.</return>
    Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId);

    /// <summary>
    /// Retrieves a collection of <see cref="ReviewDto"/> associated with a specific user email address.
    /// </summary>
    /// <param name="email">The email address of the user whose reviews are to be retrieved.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ReviewDto"/> associated with the specified email address.</return>
    Task<IEnumerable<ReviewDto>> GetByUserEmailAsync(string email);

    /// <summary>
    /// Retrieves a collection of <see cref="ReviewDto"/> associated with a specific anime identifier.
    /// </summary>
    /// <param name="animeId">The unique identifier of the anime whose reviews are to be retrieved.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ReviewDto"/> associated with the specified anime.</return>
    Task<IEnumerable<ReviewDto>> GetByAnimeIdAsync(int animeId);

    /// <summary>
    /// Retrieves a collection of <see cref="ReviewDto"/> associated with a specific anime title.
    /// </summary>
    /// <param name="title">The title of the anime whose reviews are to be retrieved.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ReviewDto"/> associated with the specified anime title.</return>
    Task<IEnumerable<ReviewDto>> GetByTitleAsync(string title);

    /// <summary>
    /// Retrieves a collection of <see cref="ReviewDto"/> created on a specific date.
    /// </summary>
    /// <param name="date">The date to filter reviews by.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ReviewDto"/> created on the specified date.</return>
    Task<IEnumerable<ReviewDto>> GetByDateAsync(DateTime date);

    /// <summary>
    /// Retrieves a collection of the most recent <see cref="ReviewDto"/> objects within a specified time span.
    /// </summary>
    /// <param name="timeSpan">The time span within which to retrieve the most recent reviews.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ReviewDto"/> objects created within the specified time span.</return>
    Task<IEnumerable<ReviewDto>> GetMostRecentByTimeSpanAsync(TimeSpan timeSpan);

    /// <summary>
    /// Retrieves a collection of <see cref="ReviewDto"/> that have a score greater than or equal to the specified minimum score.
    /// </summary>
    /// <param name="minScore">The minimum score threshold for retrieving reviews.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains a collection of <see cref="ReviewDto"/> that meet the minimum score condition.</return>
    Task<IEnumerable<ReviewDto>> GetByMinScoreAsync(int minScore);

    /// <summary>
    /// Creates a new <see cref="ReviewDto"/> in the system.
    /// </summary>
    /// <param name="entity">The <see cref="ReviewDto"/> object to create.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains the created <see cref="ReviewDto"/> if successful, or null if the creation fails.</return>
    Task<ReviewDto?> CreateAsync(ReviewDto entity);

    /// <summary>
    /// Updates an existing <see cref="ReviewDto"/> in the system.
    /// </summary>
    /// <param name="entity">The <see cref="ReviewDto"/> containing the updated information.</param>
    /// <return>A task that represents the asynchronous operation. The task result contains the updated <see cref="ReviewDto"/> if the update was successful, or null if not.</return>
    Task<ReviewDto?> UpdateAsync(ReviewDto entity);

    /// <summary>
    /// Deletes a <see cref="ReviewDto"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review to delete.</param>
    /// <return>A task that represents the asynchronous operation. The task result is true if the deletion was successful, or false otherwise.</return>
    Task<bool> DeleteAsync(int id);
}