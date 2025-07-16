using System.ComponentModel.DataAnnotations.Schema;
using AnimeApi.Server.Core.Abstractions.DataAccess.Models;

namespace AnimeApi.Server.Core.Objects.Models;

public partial class AnimeLicensor : IAnimeRelation
{
    public int Id { get; set; }

    public int AnimeId { get; set; }

    public int LicensorId { get; set; }
    [NotMapped]
    public int RelatedId
    {
        get => LicensorId;
        set => LicensorId = value;
    }

    public virtual Anime Anime { get; set; } = null!;

    public virtual Licensor Licensor { get; set; } = null!;
}