using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Auth;

public record FacebookToken
{
    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; init; } = string.Empty;
    
    [JsonProperty(PropertyName = "token_type")]
    public string TokenType { get; init; } = string.Empty;
    
    [JsonProperty(PropertyName = "expires_in")]
    public int ExpiresIn { get; init; }
}