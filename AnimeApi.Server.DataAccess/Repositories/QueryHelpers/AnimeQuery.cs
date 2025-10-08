
using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories.QueryHelpers;
public class AnimeQuery : Query<Anime>
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

    public AnimeQuery IncludeReviews()
    {
       _query = _query.Include(a => a.Reviews);
        return this;
    }
    
    public AnimeQuery IncludeFullRelation()
    {
        return IncludeProducers()
               .IncludeLicensors()
               .IncludeGenres()
               .IncludeSource()
               .IncludeType()
               .IncludeReviews();
    }
}
