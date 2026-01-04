namespace AnimeApi.Server.Core.Objects.Models;

public class Favourite
{
    public int UserId { get; set; }
    public int AnimeId { get; set; }
    
    public AppUser User { get; set; } = null!;
    
    public Anime Anime { get; set; } = null!;
}