using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IRefreshTokenHelper
{
    /// <summary>
    /// Retrieves a refresh token by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the refresh token to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="RefreshTokenDto"/> if found; otherwise, null.</returns>
    Task<RefreshTokenDto?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a refresh token using the plain token string.
    /// </summary>
    /// <param name="token">The token string used to locate the refresh token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="RefreshTokenDto"/> if found; otherwise, null.</returns>
    Task<RefreshTokenDto?> GetByTokenAsync(string token);

    /// <summary>
    /// Retrieves a refresh token associated with a specific user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose refresh token is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="RefreshTokenDto"/> if found; otherwise, null.</returns>
    Task<RefreshTokenDto?> GetByUserIdAsync(int userId);

    /// <summary>
    /// Adds a new refresh token to the system.
    /// </summary>
    /// <param name="refreshToken">The <see cref="RefreshTokenDto"/> object representing the refresh token to be added.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added <see cref="RefreshTokenDto"/>.</returns>
    Task<RefreshTokenDto?> AddAsync(RefreshTokenDto refreshToken);

    /// <summary>
    /// Revokes an existing refresh token using the provided token string.
    /// </summary>
    /// <param name="token">The token string identifying the refresh token to revoke.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is true if the token was successfully revoked; otherwise, false.</returns>
    Task<bool> RevokeAsync(string token);

    /// <summary>
    /// Revokes a refresh token associated with a specific user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose refresh token is to be revoked.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the operation was successful.</returns>
    Task<bool> RevokeByUserIdAsync(int userId);

    /// <summary>
    /// Deletes a refresh token by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the refresh token to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Deletes a refresh token associated with a specific user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose refresh token is to be deleted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result returns true if the refresh token was successfully deleted; otherwise, false.
    /// </returns>
    Task<bool> DeleteByUserIdAsync(int userId);
}