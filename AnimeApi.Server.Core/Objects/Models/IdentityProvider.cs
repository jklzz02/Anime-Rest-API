namespace AnimeApi.Server.Core.Objects.Models;

public partial class IdentityProvider
{
    public int Id { get; set; }

    public string Name { get; set; } = null;
    
    public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}