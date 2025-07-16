namespace AnimeApi.Server.Core.Objects.Models;

public class Role
{
    public int Id { get; set; }
    public string Access { get; set; } = null!;
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}