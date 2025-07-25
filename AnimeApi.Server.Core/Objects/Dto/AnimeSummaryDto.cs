using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

public record AnimeSummaryDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("title")]
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    [JsonProperty("score")]
    public decimal Score { get; set; }
    [JsonProperty("rating")]
    public string Rating { get; set; } = null!;
    [JsonProperty("release_year")]
    public int ReleaseYear { get; set; }
}