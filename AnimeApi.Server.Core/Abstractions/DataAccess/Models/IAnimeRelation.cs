namespace AnimeApi.Server.Core.Abstractions.DataAccess.Models;

/// <summary>
/// Represents a relation between an anime entity and another related entity.
/// </summary>
public interface IAnimeRelation
{
    public int AnimeId { get; set; }
    public int RelatedId { get; set; }
}