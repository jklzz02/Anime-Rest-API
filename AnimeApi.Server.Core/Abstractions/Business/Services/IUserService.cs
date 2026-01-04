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
    /// Retrieves a user by their email address asynchronously.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>
    /// An <see cref="AppUserDto"/> representing the user if found, or null if no user exists with the specified email.
    /// </returns>
    Task<AppUserDto?> GetByEmailAsync(string email);
    
    /// <summary>
    /// Retrieves a user by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>
    /// An <see cref="AppUserDto"/> representing the user if found, or null if no user exists with the specified identifier.
    /// </returns>
    Task<AppUserDto?> GetByIdAsync(int id);
    
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
    /// An <see cref="AppUserDto"/> representing the retrieved or newly created user.
    /// </returns>
    Task<AppUserDto> GetOrCreateUserAsync(AuthPayload payload);

    /// <summary>
    /// Deletes a user identified by their email address asynchronously.
    /// </summary>
    /// <param name="email">The email address of the user to be deleted.</param>
    /// <returns>
    /// A boolean value indicating whether the user was successfully deleted
    /// (true if deleted, false if the user does not exist).
    /// </returns>
    Task<bool> DestroyUserAsync(string email);
}