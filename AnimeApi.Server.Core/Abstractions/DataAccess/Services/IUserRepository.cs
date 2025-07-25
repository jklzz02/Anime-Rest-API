using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves an <see cref="AppUser"/> based on the provided email.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>An <see cref="AppUser"/> if a user with the specified email exists; otherwise, null.</returns>
    Task<AppUser?> GetByEmailAsync(string email);
    /// <summary>
    /// Retrieves an <see cref="AppUser"/> based on the provided ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>An <see cref="AppUser"/> if a user with the specified ID exists; otherwise, null.</returns>
    Task<AppUser?> GetByIdAsync(int id);
    /// <summary>
    /// Creates a new user in the data store.
    /// </summary>
    /// <param name="user">The <see cref="AppUser"/> instance containing user details to be added.</param>
    /// <returns>A boolean indicating whether the user was successfully created.</returns>
    Task<bool?> CreateAsync(AppUser user);

    /// <summary>
    /// Deletes an <see cref="AppUser"/> from the data store based on the provided email address.
    /// </summary>
    /// <param name="email">The email address of the user to be deleted.</param>
    /// <returns>A boolean indicating whether the user was successfully deleted.</returns>
    Task<bool> DestroyAsync(string email);
}