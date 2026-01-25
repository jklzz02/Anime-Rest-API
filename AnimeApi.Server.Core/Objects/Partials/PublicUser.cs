using AnimeApi.Server.Core.Abstractions.Dto;
using AnimeApi.Server.Core.Objects.Dto;
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Partials;

public record PublicUser : IProjectableFrom<AppUserDto>
{
    [JsonProperty("id")]
    public int Id { get; init; }
    
    [JsonProperty("username")]
    public string Username { get; init; } =  string.Empty;
    
    [JsonProperty("picture_url")]
    public string PictureUrl { get; init; } =  string.Empty;
}