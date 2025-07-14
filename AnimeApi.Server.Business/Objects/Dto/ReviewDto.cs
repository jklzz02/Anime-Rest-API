using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Objects.Dto;

public record ReviewDto
{
    [JsonProperty("id")]
    public int Id { get; init; }
    [JsonProperty("content")]
    public string Content { get; init; } = "";
    [JsonProperty("score")]
    public decimal Score { get; init; }
    [JsonProperty("anime_id")]
    public int AnimeId { get; init; }
    [JsonProperty("user_id")]
    public int UserId { get; init; }
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; init; }
    [JsonProperty("updated_at")]
    public AppUserDto User { get; init; } = null!;
    [JsonProperty("anime")]
    public AnimeDto Anime { get; init; } = null!;
}