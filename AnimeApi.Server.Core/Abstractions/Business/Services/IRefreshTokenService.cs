using AnimeApi.Server.Core.Objects;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IRefreshTokenService
{
    /// <summary>
    /// Asynchronously creates a refresh token for the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the refresh token is to be generated.</param>
    /// <returns>An instance of <see cref="RefreshTokenResult"/> containing the generated token and related metadata.</returns>
    Task<RefreshTokenResult> CreateAsync(int userId);

    /// <summary>
    /// Asynchronously validates the provided refresh token.
    /// </summary>
    /// <param name="token">The refresh token to be validated.</param>
    /// <returns>An instance of <see cref="RefreshTokenValidation"/> indicating the validation result and associated user information.</returns>
    Task<RefreshTokenValidation> ValidateAsync(string token);

    /// <summary>
    /// Asynchronously revokes the specified refresh token.
    /// </summary>
    /// <param name="token">The refresh token to be revoked.</param>
    /// <returns>A boolean value indicating whether the revocation was successful.</returns>
    Task<bool> RevokeAsync(string token);
}