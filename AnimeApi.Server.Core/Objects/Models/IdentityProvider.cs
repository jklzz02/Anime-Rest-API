using AnimeApi.Server.Core.Abstractions.DataAccess.Models;

namespace AnimeApi.Server.Core.Objects.Models;

public partial class IdentityProvider : IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null;
    
    public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}