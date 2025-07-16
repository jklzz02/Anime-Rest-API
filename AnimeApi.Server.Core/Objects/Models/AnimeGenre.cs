using System.ComponentModel.DataAnnotations.Schema;
using AnimeApi.Server.Core.Abstractions.DataAccess.Models;

namespace AnimeApi.Server.DataAccess.Models;

public partial class AnimeGenre : IAnimeRelation
{
    public int Id { get; set; }

    public int AnimeId { get; set; }

    public int GenreId { get; set; }
    [NotMapped]
    public int RelatedId
    {
        get => GenreId;
        set => GenreId = value;
    }
    public virtual Anime Anime { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}