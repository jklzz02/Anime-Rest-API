using System.ComponentModel.DataAnnotations.Schema;
using AnimeApi.Server.Core.Abstractions.DataAccess.Models;

namespace AnimeApi.Server.DataAccess.Models;

public partial class AnimeProducer : IAnimeRelation
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