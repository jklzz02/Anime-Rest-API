using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.Core.Objects.Partials;

namespace AnimeApi.Server.Core.Objects.Dto;

public record ReviewDetailedDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int AnimeId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Score { get; set; }
    public PublicUser User { get; init; } = new();
    public AnimeSummary Anime { get; init; } = new();
}