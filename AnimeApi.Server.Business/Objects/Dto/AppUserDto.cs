using System.Text.Json.Serialization;

namespace AnimeApi.Server.Business.Objects.Dto;

public record AppUserDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("username")]
    public required string Username { get; init; }
    
    [JsonPropertyName("email")]
    public required string Email { get; init; }
    
    [JsonPropertyName("profile_picture")]
    public required string ProfilePictureUrl { get; init; }
    
    [JsonPropertyName("created_at")]
    public required DateTime CreatedAt { get; init; }
    
    [JsonPropertyName("admin")]
    public bool Admin { get; init; }
}