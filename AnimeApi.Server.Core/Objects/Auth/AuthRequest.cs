using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Auth;

/// <summary>
/// Represents a request object used for login functionality.
/// </summary>
/// <remarks>
/// This class used to capture the required data from the client when making a login request
/// token which is used to validate and authenticate the user through OAuth identity providers
/// authentication services before creating or fetching the user.
/// </remarks>
public class AuthRequest
{
    /// <summary>
    /// The unique access token.
    /// </summary>
    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// The targeted identity provider.
    /// </summary>
    [JsonProperty("provider")]
    public Constants.Auth.IdentityProvider Provider { get; set; }
}