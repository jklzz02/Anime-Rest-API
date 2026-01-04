namespace AnimeApi.Server.Core.Objects.Models;

public class Review
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int AnimeId { get; set; }
    public Anime Anime { get; set; } = null!;
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public decimal Score { get; set; }
}