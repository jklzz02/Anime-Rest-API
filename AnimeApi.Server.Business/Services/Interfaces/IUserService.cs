using AnimeApi.Server.Business.Objects.Dto;
using Google.Apis.Auth;

namespace AnimeApi.Server.Business.Services.Interfaces;

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
    /// Retrieves an existing user or creates a new one based on the provided Google authentication payload.
    /// </summary>
    /// <param name="payload">The Google authentication payload containing user details.</param>
    /// <returns>
    /// An <see cref="AppUserDto"/> representing the retrieved or newly created user.
    /// </returns>
    Task<AppUserDto> GetOrCreateUserAsync(GoogleJsonWebSignature.Payload payload);

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