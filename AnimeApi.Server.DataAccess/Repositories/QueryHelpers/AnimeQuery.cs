
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.DataAccess.Repositories.QueryHelpers;
public class AnimeQuery : Query<Anime>
{
    public AnimeQuery(IQueryable<Anime> query) : 
        base(query)
    {
    }

    public AnimeQuery IncludeProducers()
    {
        Include(a => a.Anime_Producers)
            .ThenInclude<AnimeProducer, Producer>(ap => ap.Producer);
        
        return this;
    }

    public AnimeQuery IncludeLicensors()
    {
        Include(a => a.Anime_Licensors)
            .ThenInclude<AnimeLicensor, Licensor>(al => al.Licensor);
        
        return this;
    }

    public AnimeQuery IncludeGenres()
    {
        Include(a => a.Anime_Genres)
            .ThenInclude<AnimeGenre, Genre>(ag => ag.Genre);

        return this;
    }

    public AnimeQuery IncludeSource()
    {
        Include(a => a.Source);
        return this;
    }

    public AnimeQuery IncludeType()
    {
        Include(a => a.Type);
        return this;
    }

    public AnimeQuery IncludeReviews()
    {
        Include(a => a.Reviews);
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
