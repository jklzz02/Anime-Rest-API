namespace AnimeApi.Server.Core.Objects.Models;

public partial class Source
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Anime> Animes { get; set; } = new List<Anime>();
}
