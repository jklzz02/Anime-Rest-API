using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.QueryHelpers;
public class AnimeQuery : Query<Anime, AnimeQuery>
{
    public AnimeQuery(IQueryable<Anime> query)
        : base(query)
    {
    }

    public AnimeQuery IncludeProducers()
    {
        _query = _query.Include(a => a.Anime_Producers)
                       .ThenInclude(ap => ap.Producer);
        return this;
    }

    public AnimeQuery IncludeLicensors()
    {
        _query = _query.Include(a => a.Anime_Licensors)
                       .ThenInclude(al => al.Licensor);
        return this;
    }

    public AnimeQuery IncludeGenres()
    {
        _query = _query.Include(a => a.Anime_Genres)
                       .ThenInclude(ag => ag.Genre);
        return this;
    }

    public AnimeQuery IncludeSource()
    {
        _query = _query.Include(a => a.Source);
        return this;
    }

    public AnimeQuery IncludeType()
    {
        _query = _query.Include(a => a.Type);
        return this;
    }

    public AnimeQuery IncludeFavourites()
    {
        _query = _query.Include(a => a.Favourites);
        return this;
    }

    public AnimeQuery IncludeReviews()
    {
        _query = _query.Include(a => a.Reviews);
        return this;
    }

    public AnimeQuery IncludeFullRelation()
    {
        IncludeProducers();
        IncludeLicensors();
        IncludeGenres();
        IncludeSource();
        IncludeType();
        IncludeFavourites();
        IncludeReviews();

        return this;
    }
}
