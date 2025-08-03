namespace AnimeApi.Server.Core.Objects.Models;

public class UserFavourites
{
    public int User_Id { get; set; }
    public int Anime_Id { get; set; }
    
    public AppUser User { get; set; } = null!;
    
    public Anime Anime { get; set; } = null!;
}