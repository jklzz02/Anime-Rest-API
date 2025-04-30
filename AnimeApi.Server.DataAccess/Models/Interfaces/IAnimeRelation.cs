namespace AnimeApi.Server.DataAccess.Models.Interfaces;

public interface IAnimeRelation
{
    public int AnimeId { get; set; }
    public int RelatedId { get; set; }
}