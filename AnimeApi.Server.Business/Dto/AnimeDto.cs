using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Dto;

public record AnimeDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("title")]
    public string Name { get; set; } = null!;
    [JsonProperty("english_title")]
    public string EnglishName { get; set; } = null!;
    [JsonProperty("other_title")]
    public string OtherName { get; set; } = null!;
    [JsonProperty("synopsis")]
    public string Synopsis { get; set; } = null!;
    [JsonProperty("image_url")]
    public string ImageUrl { get; set; } = null!;
    [JsonProperty("type")]
    public string Type { get; set; } = null!;
    [JsonProperty("episodes")]
    public int Episodes { get; set; }
    [JsonProperty("duration")]
    public string Duration { get; set; } = null!;
    [JsonProperty("source")]
    public string Source { get; set; } = null!;
    [JsonProperty("release_year")]
    public int ReleaseYear { get; set; }
    [JsonProperty("started_airing")]
    public DateOnly? StartedAiring { get; set; }
    [JsonProperty("finished_airing")]
    public DateOnly? FinishedAiring { get; set; }
    [JsonProperty("rating")]
    public string Rating { get; set; } = null!;
    [JsonProperty("studio")]
    public string Studio { get; set; } = null!;
    [JsonProperty("score")]
    public int Score { get; set; }
    [JsonProperty("status")]
    public string Status { get; set; } = null!;
    [JsonProperty("genres")]
    public List<GenreDto> Genres { get; set; } = [];
    [JsonProperty("licensors")]
    public List<LicensorDto> Licensors { get; set; } = [];
    [JsonProperty("producers")]
    public List<ProducerDto> Producers { get; set; } = [];
}