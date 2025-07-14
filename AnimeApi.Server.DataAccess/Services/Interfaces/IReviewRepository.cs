using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.DataAccess.Services.Interfaces;

/// <summary>
/// Interface for accessing and managing <see cref="Review"/> data.
/// </summary>
public interface IReviewRepository
{
    /// <summary>
    /// A dictionary containing error messages associated with specific error keys.
    /// This property serves as a centralized storage for predefined error messages
    /// that may be used across the repository implementation for consistency and reusability.
    /// </summary>
    Dictionary<string, string> ErrorMessages { get; }

    /// <summary>
    /// Asynchronously retrieves a <see cref="Review"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Review"/> to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Review"/> if found; otherwise, null.</returns>
    Task<Review?> GetByIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves a collection of <see cref="Review"/> instances associated with a specific anime.
    /// </summary>
    /// <param name="id">The unique identifier of the anime for which reviews are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Review"/> instances associated with the specified anime.</returns>
    Task<IEnumerable<Review>> GetByAnimeIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves a collection of <see cref="Review"/> instances associated with a specific anime title.
    /// </summary>
    /// <param name="title">The title of the anime for which reviews are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Review"/> instances associated with the specified anime title.</returns>
    Task<IEnumerable<Review>> GetByAnimeTitleAsync(string title);

    /// <summary>
    /// Asynchronously retrieves a collection of <see cref="Review"/> instances associated with a specific user.
    /// </summary>
    /// <param name="id">The unique identifier of the user for which reviews are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Review"/> instances associated with the specified user.</returns>
    Task<IEnumerable<Review>> GetByUserIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves a collection of <see cref="Review"/> instances associated with a specific user's email.
    /// </summary>
    /// <param name="email">The email address of the user whose reviews are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Review"/> instances associated with the specified email.</returns>
    Task<IEnumerable<Review>> GetByUserEmailAsync(string email);

    /// <summary>
    /// Asynchronously retrieves a collection of <see cref="Review"/> instances created on a specified date.
    /// </summary>
    /// <param name="date">The date for which reviews are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Review"/> instances created on the specified date.</returns>
    Task<IEnumerable<Review>> GetByDateAsync(DateTime date);

    /// <summary>
    /// Asynchronously retrieves a collection of the most recent <see cref="Review"/> instances within the specified timespan.
    /// </summary>
    /// <param name="timespan">The timespan within which to retrieve the most recent reviews.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Review"/> instances created within the specified timespan.</returns>
    Task<IEnumerable<Review>> GetMostRecentByTimespanAsync(TimeSpan timespan);

    /// <summary>
    /// Asynchronously retrieves a collection of <see cref="Review"/> instances with a score greater than or equal to the specified minimum score.
    /// </summary>
    /// <param name="minScore">The minimum score that a <see cref="Review"/> must have to be included in the results.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="Review"/> instances that meet the score criteria.</returns>
    Task<IEnumerable<Review>> GetByMinScoreAsync(int minScore);

    /// <summary>
    /// Asynchronously creates a new <see cref="Review"/>.
    /// </summary>
    /// <param name="review">The <see cref="Review"/> instance to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="Review"/> if the operation is successful; otherwise, null.</returns>
    Task<Review?> CreateAsync(Review review);

    /// <summary>
    /// Asynchronously updates an existing <see cref="Review"/> in the data store.
    /// </summary>
    /// <param name="review">The <see cref="Review"/> object containing updated information to be saved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated <see cref="Review"/> if successful; otherwise, null.</returns>
    Task<Review?> UpdateAsync(Review review);

    /// <summary>
    /// Asynchronously deletes a <see cref="Review"/> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Review"/> to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
    Task<bool> DeleteAsync(int id);
}