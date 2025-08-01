using AnimeApi.Server.Core.Abstractions.DataAccess.Models;

namespace AnimeApi.Server.Core.Objects.Models;

public partial class Licensor : IBaseModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AnimeLicensor> Anime_Licensors { get; set; } = new List<AnimeLicensor>();
}
