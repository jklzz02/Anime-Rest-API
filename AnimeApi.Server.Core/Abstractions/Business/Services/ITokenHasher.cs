namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface ITokenHasher
{
    /// <summary>
    /// Hashes the provided token and returns the resulting hashed string.
    /// </summary>
    /// <param name="token">The token to be hashed.</param>
    /// <returns>A hashed representation of the input token.</returns>
    string Hash(string token);
    
    /// <summary>
    /// Verifies that the provided token matches the hashed value.
    /// </summary>
    /// <param name="token">The token to be verified.</param>
    /// <param name="hashedToken">The hashed token to be compared against.</param>
    /// <returns>A boolean value indicating whether the token matches the hashed value.</returns>
    bool Verify(string token, string hashedToken);
}