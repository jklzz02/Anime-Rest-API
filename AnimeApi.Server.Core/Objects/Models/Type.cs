using AnimeApi.Server.Core.Abstractions.DataAccess.Models;

namespace AnimeApi.Server.Core.Objects.Models;

public partial class Type : IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Anime> Anime { get; set; } = new List<Anime>();
}
