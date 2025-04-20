namespace AnimeApi.Server.Business.Dto;

public record AnimeDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string EnglishName { get; set; } = null!;

    public string OtherName { get; set; } = null!;

    public string Synopsis { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int Episodes { get; set; }

    public string Duration { get; set; } = null!;

    public string Source { get; set; } = null!;

    public int ReleaseYear { get; set; }

    public DateOnly? StartedAiring { get; set; }

    public DateOnly? FinishedAiring { get; set; }

    public string Rating { get; set; } = null!;

    public string Studio { get; set; } = null!;

    public int Score { get; set; }

    public string Status { get; set; } = null!;

    public List<GenreDto> Genres { get; set; } = [];

    public List<LicensorDto> Licensors { get; set; } = [];

    public List<ProducerDto> Producers { get; set; } = [];
}