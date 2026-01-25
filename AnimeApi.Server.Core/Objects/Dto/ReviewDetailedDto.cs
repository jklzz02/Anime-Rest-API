using AnimeApi.Server.Core.Abstractions.Dto;
using AnimeApi.Server.Core.Objects.Partials;
using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record ReviewDetailedDto : IProjectableFrom<ReviewDto>
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
    
    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;
    
    [JsonProperty("anime_id")]
    public int AnimeId { get; set; }
    
    [JsonProperty("user_id")]
    public int UserId { get; set; }
    
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [JsonProperty("score")]
    public decimal Score { get; set; }
    
    [JsonProperty("user")]
    public PublicUser User { get; init; } = new();
    
    [JsonProperty("anime")]
    public AnimeSummary Anime { get; init; } = new();
}