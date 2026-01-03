using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Auth;

public record DiscordResponse
{
    [JsonProperty("access_token")]
    public string Id { get; init; } = string.Empty;
    
    [JsonProperty("username")]
    public string Username { get; init; } = string.Empty;
    
    [JsonProperty("email")]
    public string Email { get; init; } = string.Empty;
    
    [JsonProperty("avatar")]
    public string? Avatar { get; init; } = string.Empty;
    
    public string AvatarUrl
        => Avatar is null
            ? string.Empty 
            : $"https://cdn.discordapp.com/avatars/{Id}/{Avatar}.png";
}