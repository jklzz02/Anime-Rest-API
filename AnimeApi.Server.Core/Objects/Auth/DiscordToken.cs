using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Auth;

public record DiscordToken
{
    [JsonProperty("access_token")] 
    public string AccessToken { get; init; } = string.Empty;
    
    [JsonProperty("token_type")]
    public string TokenType  { get; init; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; init; } = string.Empty;

    [JsonProperty("scope")]
    public string Scope { get; init; } = string.Empty;
}