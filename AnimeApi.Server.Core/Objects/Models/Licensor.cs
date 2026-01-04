using AnimeApi.Server.Core.Abstractions.DataAccess.Models;

namespace AnimeApi.Server.Core.Objects.Models;

public partial class Licensor : IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AnimeLicensor> AnimeLicensors { get; set; } = new List<AnimeLicensor>();
}
