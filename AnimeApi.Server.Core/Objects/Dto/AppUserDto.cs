
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record AppUserDto
{
    [JsonProperty("id")]
    public int Id { get; init; }

    [JsonProperty("username")]
    public required string Username { get; init; }
    
    [JsonProperty("email")]
    public required string Email { get; init; }
    
    [JsonProperty("profile_picture")]
    public required string ProfilePictureUrl { get; init; }
    
    [JsonProperty("created_at")]
    public required DateTime CreatedAt { get; init; }
    
    [JsonProperty("admin")]
    public bool Admin { get; init; }
}