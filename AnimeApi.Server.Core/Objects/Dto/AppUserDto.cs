
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record AppUserDto
{
    [JsonProperty("id")]
    public int Id { get; init; }

    [JsonProperty("username")]
    public string Username { get; init; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; init; } = string.Empty;

    [JsonProperty("profile_picture")]
    public string ProfilePictureUrl { get; init; } = string.Empty;
    
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; init; }
    
    [JsonProperty("admin")]
    public bool Admin { get; init; }
}