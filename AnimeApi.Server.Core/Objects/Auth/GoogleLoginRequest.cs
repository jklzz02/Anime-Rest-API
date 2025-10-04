using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Auth;

/// <summary>
/// Represents a request object used for Google login functionality.
/// </summary>
/// <remarks>
/// This class used to capture the required data from the client when making a login request
/// using a Google ID token. The ID token is used to validate and authenticate the user through Google's
/// authentication services before creating or fetching the user within the application.
/// </remarks>
public class GoogleLoginRequest
{
    [JsonProperty("id_token")]
    public string IdToken { get; set; } = string.Empty;
}