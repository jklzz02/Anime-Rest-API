using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Sorting;

public class AnimeSortMap : SortMap<Anime>
{
    public AnimeSortMap()
    {
        Register(SortConstants.Anime.Id, SortAction<Anime>.Desc(a => a.Id));
        Register(SortConstants.Anime.Name, SortAction<Anime>.Asc(a => a.Name));
        Register(SortConstants.Anime.Score, SortAction<Anime>.Desc(a => a.Score));
        Register(SortConstants.Anime.ReleaseYear, SortAction<Anime>.Desc(a => a.ReleaseYear));
        Register(SortConstants.Anime.ReleaseDate, SortAction<Anime>.Desc(a => a.StartedAiring));
        Register(SortConstants.Anime.Episodes, SortAction<Anime>.Asc(a => a.Episodes));
    }
}