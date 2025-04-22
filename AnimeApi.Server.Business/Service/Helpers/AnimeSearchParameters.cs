namespace AnimeApi.Server.Business.Service.Helpers;

public class AnimeSearchParameters
{
    public string? Name { get; set; } = null;
    public int? ProducerId { get; set; } = null;
    public int? LicensorId { get; set; } = null;
    public int? GenreId { get; set; } = null;
    public string? Source { get; set; } = null;
    public string? Type { get; set; } = null;
    public string? EnglishName { get; set; } = null;
    public int? MinScore { get; set; } = null;
    public int? MaxScore { get; set; } = null;
    public int? MinReleaseYear { get; set; } = null;
    public int? MaxReleaseYear { get; set; } = null;
}