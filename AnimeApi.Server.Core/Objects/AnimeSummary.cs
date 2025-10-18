namespace AnimeApi.Server.Core.Objects;

public class AnimeSummary
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public decimal Score { get; set; }
    public string Rating { get; set; } = null!;
    public int ReleaseYear { get; set; }
}