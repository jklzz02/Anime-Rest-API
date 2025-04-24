using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Business.Service.Helpers;

public class AnimeSearchParameters
{
    [FromRoute(Name = "title")]
    public string? Name { get; set; } = null;
     [FromQuery(Name = "producer_id")]
    public int? ProducerId { get; set; } = null;
    [FromQuery(Name = "producer")]
    public string? ProducerName { get; set; } = null;
    [FromQuery(Name = "licensor_id")]
    public int? LicensorId { get; set; } = null;
    [FromQuery(Name = "licensor")]
    public string? LicensorName { get; set; } = null;
    [FromQuery(Name = "genre_id")]
    public int? GenreId { get; set; } = null;
    [FromQuery(Name = "genre")]
    public string? GenreName { get; set; } = null;
    [FromQuery(Name = "status")]
    public string? Status { get; set; } = null;
    [FromQuery(Name = "episodes")]
    public int? Episodes { get; set; } = null;
    [FromQuery(Name = "source")]
    public string? Source { get; set; } = null;
    [FromQuery(Name = "type")]
    public string? Type { get; set; } = null;
    [FromQuery(Name = "english_name")]
    public string? EnglishName { get; set; } = null;
    [FromQuery(Name = "min_score")]
    public int? MinScore { get; set; } = null;
    [FromQuery(Name = "max_score")]
    public int? MaxScore { get; set; } = null;
    [FromQuery(Name = "min_release_year")]
    public int? MinReleaseYear { get; set; } = null;
    [FromQuery(Name = "max_release_year")]
    public int? MaxReleaseYear { get; set; } = null;
}