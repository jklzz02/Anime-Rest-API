using Newtonsoft.Json;

namespace AnimeApi.Server.Core.Objects.Dto;

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
    [JsonProperty("background")]
    public string? Background { get; set; }
    [JsonProperty("status")]
    public string? Status { get; set; }
    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }
    [JsonProperty("type")]
    public TypeDto? Type { get; set; }
    [JsonProperty("episodes")]
    public int Episodes { get; set; }
    [JsonProperty("duration")]
    public string? Duration { get; set; }
    [JsonProperty("source")]
    public SourceDto? Source { get; set; }
    [JsonProperty("release_year")]
    public int ReleaseYear { get; set; }
    [JsonProperty("started_airing")]
    public DateTime? StartedAiring { get; set; }
    [JsonProperty("finished_airing")]
    public DateTime? FinishedAiring { get; set; }
    [JsonProperty("rating")]
    public string? Rating { get; set; }
    [JsonProperty("studio")]
    public string? Studio { get; set; }
    [JsonProperty("score")]
    public decimal Score { get; set; }
    [JsonProperty("trailer_url")]
    public string? TrailerUrl { get; set; }
    [JsonProperty("trailer_embed_url")]
    public string? TrailerEmbedUrl { get; set; }
    [JsonProperty("trailer_image_url")]
    public string? TrailerImageUrl { get;set; }
    [JsonProperty("genres")]
    public List<GenreDto> Genres { get; set; } = [];
    [JsonProperty("licensors")]
    public List<LicensorDto> Licensors { get; set; } = [];
    [JsonProperty("producers")]
    public List<ProducerDto> Producers { get; set; } = [];
}