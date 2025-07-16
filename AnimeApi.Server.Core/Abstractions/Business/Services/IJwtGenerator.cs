using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Business.Services.Interfaces;

/// <summary>
/// Provides methods for generating JSON Web Tokens (JWT) for application users.
/// </summary>
public interface IJwtGenerator
{
    /// <summary>
    /// Generates a JSON Web Token (JWT) for the provided application user.
    /// </summary>
    /// <param name="user">The application user details required to create the token.</param>
    /// <returns>A JWT as a string, containing the user's claims and other token metadata.</returns>
    string GenerateToken(AppUserDto user);
}