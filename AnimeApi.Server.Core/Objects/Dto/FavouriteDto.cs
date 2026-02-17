using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record FavouriteDto
{
    [JsonProperty("user_id")]
    public int UserId { get; init; }
    
    [JsonProperty("anime_id")]
    public int AnimeId { get; init; }
}