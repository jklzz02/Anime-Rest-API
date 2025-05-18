using System.ComponentModel.DataAnnotations.Schema;
using AnimeApi.Server.DataAccess.Models.Interfaces;

namespace AnimeApi.Server.DataAccess.Models;

public partial class Anime_Genre : IAnimeRelation
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