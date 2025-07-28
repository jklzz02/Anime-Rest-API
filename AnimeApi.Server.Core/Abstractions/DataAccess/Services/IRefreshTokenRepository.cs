using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface IRefreshTokenRepository
{
    /// <summary>
    /// Retrieves a refresh token entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the refresh token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="RefreshToken"/>
    /// if found, otherwise <c>null</c>.
    /// </returns>
    Task<RefreshToken?> GetByIdAsync(int id);
    
    /// <summary>
    /// Retrieves a refresh token entity by its hashed token value.
    /// </summary>
    /// <param name="token">The hashed token string used to identify the refresh token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="RefreshToken"/>
    /// if found, otherwise <c>null</c>.
    /// </returns>
    Task<RefreshToken?> GetByTokenAsync(string token);

    /// <summary>
    /// Retrieves a refresh token entity associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose refresh token is being retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="RefreshToken"/>
    /// if found, otherwise <c>null</c>.
    /// </returns>
    Task<RefreshToken?> GetByUserIdAsync(int userId);

    /// <summary>
    /// Adds a new refresh token entity to the data store.
    /// </summary>
    /// <param name="refreshToken">The refresh token entity to be added.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the added <see cref="RefreshToken"/>
    /// if the operation was successful, otherwise <c>null</c>.
    /// </returns>
    Task<RefreshToken?> AddAsync(RefreshToken refreshToken);

    /// <summary>
    /// Revokes a refresh token by marking it as revoked and updating the database.
    /// </summary>
    /// <param name="token">The hashed token string identifying the refresh token to be revoked.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains <c>true</c> if the token was successfully revoked;
    /// otherwise, <c>false</c> if the token was not found.
    /// </returns>
    Task<bool> RevokeAsync(string token);

    /// <summary>
    /// Revokes the refresh token associated with the specified user ID by setting its revoked timestamp.
    /// </summary>
    /// <param name="userId">The ID of the user whose refresh token is being revoked.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the token was successfully revoked; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> RevokeByUserIdAsync(int userId);

    /// <summary>
    /// Deletes a refresh token entity identified by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the refresh token to be deleted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the token was successfully deleted; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Deletes a refresh token associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose refresh token is to be deleted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is <c>true</c> if the deletion was successful;
    /// otherwise, <c>false</c>.
    /// </returns>
    Task<bool> DeleteByUserIdAsync(int userId);
}