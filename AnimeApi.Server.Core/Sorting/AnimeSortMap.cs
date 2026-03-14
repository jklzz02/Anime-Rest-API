using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Sorting;

public class AnimeSortMap : SortMap<Anime, AnimeSortMap>
{
    public AnimeSortMap()
    {
        Register(SortConstants.Anime.Id, a => a.Id);
        Register(SortConstants.Anime.Name, a => a.Name);
        Register(SortConstants.Anime.Score, a => a.Score);
        Register(SortConstants.Anime.ReleaseYear, a => a.ReleaseYear);
        Register(SortConstants.Anime.ReleaseDate, a => a.StartedAiring);
        Register(SortConstants.Anime.Episodes, a => a.Episodes);
    }
}