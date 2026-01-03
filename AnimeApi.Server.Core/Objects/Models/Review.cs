namespace AnimeApi.Server.Core.Objects.Models;

public class Review
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Anime_Id { get; set; }
    public Anime Anime { get; set; } = null!;
    public int User_Id { get; set; }
    public AppUser User { get; set; } = null!;
    public DateTime Created_At { get; set; }
    public decimal Score { get; set; }
}