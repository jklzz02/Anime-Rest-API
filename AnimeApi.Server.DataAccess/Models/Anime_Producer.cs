using System.ComponentModel.DataAnnotations.Schema;
using AnimeApi.Server.DataAccess.Models.Interfaces;

namespace AnimeApi.Server.DataAccess.Models;

public partial class Anime_Producer : IAnimeRelation
{
    public int Id { get; set; }

    public int AnimeId { get; set; }
    public int ProducerId { get; set; }
    [NotMapped]
    public int RelatedId
    {
        get => ProducerId;
        set => ProducerId = value;
    }

    public virtual Anime Anime { get; set; } = null!;

    public virtual Producer Producer { get; set; } = null!;
}