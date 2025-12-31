using System.Security.Cryptography;
using System.Text;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Exceptions;
using Microsoft.Extensions.Configuration;

namespace AnimeApi.Server.Business.Services;

public class TokenHasher : ITokenHasher
{
    private readonly string _secret;

    public TokenHasher(IConfiguration configuration)
    {
        
        ArgumentNullException.ThrowIfNull(configuration);
        ConfigurationException.ThrowIfEmpty(configuration, "Authentication:RefreshToken");
        ConfigurationException.ThrowIfEmpty(configuration, "Authentication:RefreshToken:Secret");

        _secret = configuration
            .GetSection("Authentication:RefreshToken")
            .GetSection("Secret").Value!;
    }
    
    /// <inheritdoc />
    public string Hash(string token)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }
    
    /// <inheritdoc />
    public bool Verify(string token, string hashed)
    {
        var computed = Hash(token);
        return computed == hashed;
    }
}