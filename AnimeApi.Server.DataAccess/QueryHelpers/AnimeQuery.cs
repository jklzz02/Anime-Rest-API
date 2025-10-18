using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.QueryHelpers;
public class AnimeQuery : QuerySpec<Anime, AnimeQuery>, IQuerySpec<Anime>
{
    public AnimeQuery IncludeProducers()
    {
        Include(q => q.Include(a => a.Anime_Producers)
            .ThenInclude(ap => ap.Producer));

        return this;
    }

    public AnimeQuery IncludeLicensors()
    {
        Include(q => q.Include(a => a.Anime_Licensors)
                .ThenInclude(al => al.Licensor));
        
        return this;
    }

    public AnimeQuery IncludeGenres()
    {
        Include(q => q.Include(a => a.Anime_Genres)
                .ThenInclude(ag => ag.Genre));
        
        return this;
    }

    public AnimeQuery IncludeSource()
    {
        Include(q => q.Include(a => a.Source));

        return this;
    }

    public AnimeQuery IncludeType()
    {
        Include(q => q.Include(a => a.Type));

        return this;
    }

    public AnimeQuery IncludeFavourites()
    {
        Include(q => q.Include(a => a.Favourites));
        return this;
    }

    public AnimeQuery IncludeReviews()
    {
        Include(q => q.Include(a => a.Reviews));
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
