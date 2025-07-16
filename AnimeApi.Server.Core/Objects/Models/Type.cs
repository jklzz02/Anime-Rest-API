namespace AnimeApi.Server.Core.Objects.Models;

public partial class Type
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();
}
