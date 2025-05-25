namespace AnimeApi.Server.DataAccess.Models.Interfaces;

/// <summary>
/// Represents a relation between an anime entity and another related entity.
/// </summary>
public interface IAnimeRelation
{
    public int AnimeId { get; set; }
    public int RelatedId { get; set; }
}