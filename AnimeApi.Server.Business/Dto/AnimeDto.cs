using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Dto;

public record AnimeDto
{
    [JsonProperty("id")]
    public int? Id { get; set; }
    [JsonProperty("title")]
    public string? Name{ get; set; }
    [JsonProperty("english_title")]
    public string? EnglishName { get; set; }
    [JsonProperty("other_title")]
    public string? OtherName { get; set; }
    [JsonProperty("synopsis")]
    public string? Synopsis { get; set; }
    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }
    [JsonProperty("type")]
    public string? Type { get; set; }
    [JsonProperty("episodes")]
    public int Episodes { get; set; }
    [JsonProperty("duration")]
    public string? Duration { get; set; }
    [JsonProperty("source")]
    public string? Source { get; set; }
    [JsonProperty("release_year")]
    public int ReleaseYear { get; set; }
    [JsonProperty("started_airing")]
    public DateOnly? StartedAiring { get; set; }
    [JsonProperty("finished_airing")]
    public DateOnly? FinishedAiring { get; set; }
    [JsonProperty("rating")]
    public string? Rating { get; set; }
    [JsonProperty("studio")]
    public string? Studio { get; set; }
    [JsonProperty("score")]
    public decimal Score { get; set; }
    [JsonProperty("status")]
    public string? Status { get; set; }
    [JsonProperty("genres")]
    public List<GenreDto> Genres { get; set; } = [];
    [JsonProperty("licensors")]
    public List<LicensorDto> Licensors { get; set; } = [];
    [JsonProperty("producers")]
    public List<ProducerDto> Producers { get; set; } = [];
}