using AnimeApi.Server.Core.Abstractions.Dto;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Auth;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Partials;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

/// <summary>
/// Defines the contract for user-related operations within the application.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves a user by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>
    /// An <see cref="AppUserDto"/> representing the user if found, or null if no user exists with the specified identifier.
    /// </returns>
    Task<AppUserDto?> GetByIdAsync(int id);
    
    /// <summary>
    /// Retrieves a user by their email address asynchronously.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>
    /// An <see cref="AppUserDto"/> representing the user if found, or null if no user exists with the specified email.
    /// </returns>
    Task<AppUserDto?> GetByEmailAsync(string email);
    
    /// <summary>
    /// Retrieves a user by their email address asynchronously.
    /// </summary>
    /// <param name="email">The email address of the linked users to retrieve.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> representing the users linked to the specified email.
    /// </returns>
    Task<IEnumerable<AppUserDto>> GetUsersLinkedToEmail(string email);

    /// <summary>
    /// Retrieves a paginated list of publicly available user's data. 
    /// </summary>
    /// <param name="page">The page to fetch</param>
    /// <param name="pageSize">The number of items to retrieve per page</param>
    /// <returns>A <see cref="PaginatedResult{T}"/> representing the paginated user data.</returns>
    Task<PaginatedResult<AppUserDto>> GetUsersAsync(int page, int pageSize);
    
    /// <summary>
    /// Retrieves a paginated list of publicly available user's data. 
    /// </summary>
    /// <param name="page">The page to fetch</param>
    /// <param name="pageSize">The number of items to retrieve per page</param>
    /// <typeparam name="TUser">The type to be projected to.</typeparam>
    /// <returns>A <see cref="PaginatedResult{T}"/> representing the paginated user data.</returns>
    Task<PaginatedResult<TUser>> GetUsersAsync<TUser>(int page, int pageSize)
        where TUser : class, IProjectableFrom<AppUserDto>, new();
    
    /// <summary>
    /// Retrieves publicly accessible data of a user by their unique identifier asynchronously
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>A <see cref="PublicUser"/> representing the user if found, or null if no user exists with the specified identifier.</returns>
    Task<PublicUser?> GetPublicUserAsync(int id);
    
    /// <summary>
    /// Retrieves an existing user or creates a new one based on the provided Google authentication payload.
    /// </summary>
    /// <param name="payload">The Google authentication payload containing user details.</param>
    /// <returns>
    /// An <see cref="Result{T}"/> representing the retrieved or newly created user.
    /// </returns>
    Task<Result<AppUserDto>> GetOrCreateUserAsync(AuthPayload payload);

    /// <summary>
    /// Deletes a user identified by their id asynchronously.
    /// </summary>
    /// <param name="id">The id of the user to be deleted.</param>
    /// <returns>
    /// A boolean value indicating whether the user was successfully deleted
    /// (true if deleted, false if the user does not exist).
    /// </returns>
    Task<bool> DestroyUserAsync(int id);
    
    /// <summary>
    /// Deletes a user identified by their email address asynchronously.
    /// </summary>
    /// <param name="email">The email address of the user to be deleted.</param>
    /// <returns>
    /// A boolean value indicating whether the user was successfully deleted
    /// (true if deleted, false if the user does not exist).
    /// </returns>
    Task<bool> DestroyUserAsync(string email);
    
    /// <summary>
    /// Get the favourites of the specified user
    /// </summary>
    /// <param name="userId">The user identifier</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of the user's favourites</returns>
    Task<IEnumerable<FavouriteDto>> GetFavouritesAsync(int userId);
    
    /// <summary>
    /// Add a favourite
    /// </summary>
    /// <param name="favourite">The <see cref="FavouriteDto"/> to be added.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created entry.</returns>
    Task<Result<FavouriteDto>> AddFavouriteAsync(FavouriteDto favourite);
    
    /// <summary>
    /// Remove a favourite.
    /// </summary>
    /// <param name="favourite">The favourite to be removed.</param>
    /// <returns>True if the deletion was successful, otherwise false</returns>
    Task<bool> RemoveFavouriteAsync(FavouriteDto favourite);
}