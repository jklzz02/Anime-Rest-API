using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.Core.SpecHelpers;
public class AnimeQuery : QuerySpec<Anime, AnimeQuery>, IQuerySpec<Anime>
{

    public static AnimeQuery ByPk(int id)
    {
        return new AnimeQuery()
            .FilterBy(a => a.Id == id);
    }

    public static AnimeQuery ByPk(IEnumerable<int> ids)
    {
        return new AnimeQuery()
            .FilterBy(a => ids.Contains(a.Id));
    }

    public AnimeQuery WithFullTextSearch(string? query)
    {
        if (!string.IsNullOrWhiteSpace(query))
        {
            FilterBy(a =>
                EF.Functions.TrigramsAreSimilar(query, a.Name) ||
                EF.Functions.TrigramsAreSimilar(query, a.English_Name));
        }

        return this;
    }

    public AnimeQuery WithName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            FilterBy(a => a.Name.Contains(name));

        return this;
    }

    public AnimeQuery WithEnglishName(string? englishName)
    {
        if (!string.IsNullOrWhiteSpace(englishName))
            FilterBy(a => a.English_Name.Contains(englishName));

        return this;
    }

    public AnimeQuery WithSource(int? sourceId)
        => WithSource(sourceId, null);

    public AnimeQuery WithSource(string? source)
        => WithSource(null, source);

    public AnimeQuery WithSource(int? sourceId, string? source)
    {
        if (!string.IsNullOrWhiteSpace(source))
            FilterBy(a => a.Source.Name.ToLower().Contains(source.ToLower()));

        if (sourceId.HasValue)
            FilterBy(a => a.SourceId == sourceId);

        return this;
    }

    public AnimeQuery WithType(int? typeId)
        => WithType(typeId, null);

    public AnimeQuery WithType(string? type)
        => WithType(null, type);

    public AnimeQuery WithType(int? typeId, string? type)
    {
        if (!string.IsNullOrWhiteSpace(type))
            FilterBy(a => a.Type.Name.ToLower().Contains(type.ToLower()));

        if (typeId.HasValue)
            FilterBy(a => a.TypeId == typeId);

        return this;
    }

    public AnimeQuery WithStatus(string? status)
    {
        if (!string.IsNullOrWhiteSpace(status))
            FilterBy(a => a.Status.Contains(status));

        return this;
    }

    public AnimeQuery WithStudio(string? studio)
    {
        if (!string.IsNullOrWhiteSpace(studio))
            FilterBy(a => a.Studio.ToLower() == studio.ToLower());

        return this;
    }

    public AnimeQuery WithScoreRange(decimal? minScore, decimal? maxScore)
    {
        if (minScore.HasValue)
            FilterBy(a => a.Score >= minScore);
        if (maxScore.HasValue)
            FilterBy(a => a.Score <= maxScore);

        return this;
    }

    public AnimeQuery WithEpisodeRange(int? minEpisodes, int? maxEpisodes, int? exactEpisodes = null)
    {
        if (exactEpisodes.HasValue)
        {
            FilterBy(a => a.Episodes == exactEpisodes);
        }
        else
        {
            if (minEpisodes.HasValue)
                FilterBy(a => a.Episodes >= minEpisodes);
            if (maxEpisodes.HasValue)
                FilterBy(a => a.Episodes <= maxEpisodes);
        }

        return this;
    }

    public AnimeQuery WithYearRange(int? minYear, int? maxYear)
    {
        if (minYear.HasValue)
            FilterBy(a => a.Release_Year >= minYear);
        if (maxYear.HasValue)
            FilterBy(a => a.Release_Year <= maxYear && a.Release_Year != 0);

        return this;
    }

    public AnimeQuery WithAirDateRange(
        DateTime? startFrom,
        DateTime? startTo,
        DateTime? endFrom,
        DateTime? endTo)
    {
        if (startFrom.HasValue)
            FilterBy(a => a.Started_Airing >= startFrom.Value.ToUniversalTime());
        if (startTo.HasValue)
            FilterBy(a => a.Started_Airing <= startTo.Value.ToUniversalTime());
        if (endFrom.HasValue)
            FilterBy(a => a.Finished_Airing >= endFrom.Value.ToUniversalTime());
        if (endTo.HasValue)
            FilterBy(a => a.Finished_Airing <= endTo.Value.ToUniversalTime());

        return this;
    }

    public AnimeQuery WithGenres(
        int? genreId = null,
        string? genreName = null,
        IEnumerable<string>? genreNames = null)
    {
        if (genreId.HasValue)
            FilterBy(a => a.Anime_Genres.Any(g => g.GenreId == genreId));

        if (!string.IsNullOrWhiteSpace(genreName))
            FilterBy(a => a.Anime_Genres.Any(g => g.Genre.Name.ToLower().Contains(genreName.ToLower())));

        if (genreNames?.Any() ?? false)
            FilterBy(a => genreNames.All(g => a.Anime_Genres.Any(ag => ag.Genre.Name.ToLower() == g.ToLower())));

        return this;
    }

    public AnimeQuery WithProducers(
        int? producerId = null,
        string? producerName = null,
        IEnumerable<string>? producerNames = null)
    {
        if (producerId.HasValue)
            FilterBy(a => a.Anime_Producers.Any(p => p.ProducerId == producerId));

        if (!string.IsNullOrWhiteSpace(producerName))
            FilterBy(a => a.Anime_Producers.Any(p => p.Producer.Name.ToLower().Contains(producerName.ToLower())));

        if (producerNames?.Any() ?? false)
            FilterBy(a => producerNames.All(p => a.Anime_Producers.Any(ap => ap.Producer.Name.ToLower() == p.ToLower())));

        return this;
    }

    public AnimeQuery WithLicensors(
        int? licensorId = null,
        string? licensorName = null,
        IEnumerable<string>? licensorNames = null)
    {
        if (licensorId.HasValue)
            FilterBy(a => a.Anime_Licensors.Any(l => l.LicensorId == licensorId));

        if (!string.IsNullOrWhiteSpace(licensorName))
            FilterBy(a => a.Anime_Licensors.Any(l => l.Licensor.Name.ToLower().Contains(licensorName.ToLower())));

        if (licensorNames?.Any() ?? false)
            FilterBy(a => licensorNames.All(l => a.Anime_Licensors.Any(al => al.Licensor.Name.ToLower() == l.ToLower())));

        return this;
    }

    public AnimeQuery WithSorting(string? field, string? order)
    {
        if (string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(order))
        {
            return this;
        }

        if (!Constants.OrderBy.Fields.ValidFields.Contains(field.ToLower()))
        {
            throw new ArgumentException(
                $"Invalid order by field. Choose among: ({string.Join(", ", Constants.OrderBy.Fields.ValidFields)})");
        }

        if (!Constants.OrderBy.StringDirections.Directions.Contains(order.ToLower()))
        {             
            throw new ArgumentException(
                $"Invalid sort order. Choose among: ({string.Join(", ", Constants.OrderBy.StringDirections.Directions)})");
        }

        bool ascending = order.EqualsIgnoreCase(Constants.OrderBy.StringDirections.Ascending);

        SortBy(field.ToLowerNormalized() switch
        {
            Constants.OrderBy.Fields.Id => ascending
                ? SortAction<Anime>.Asc(a => a.Id)
                : SortAction<Anime>.Desc(a => a.Id),
            Constants.OrderBy.Fields.Name => ascending
                ? SortAction<Anime>.Asc(a => a.Name)
                : SortAction<Anime>.Desc(a => a.Name),
            Constants.OrderBy.Fields.ReleaseYear => ascending
                ? SortAction<Anime>.Asc(a => a.Release_Year)
                : SortAction<Anime>.Desc(a => a.Release_Year),
            Constants.OrderBy.Fields.ReleaseDate => ascending
                ? SortAction<Anime>.Asc(a => a.Started_Airing)
                : SortAction<Anime>.Desc(a => a.Started_Airing),
            _ => ascending
                ? SortAction<Anime>.Asc(a => a.Score)
                : SortAction<Anime>.Desc(a => a.Score),
        });

        return this;
    }

    public AnimeQuery Recents(int count)
    {
        if (count <= 0)
            throw new InvalidOperationException("Count must be greater than 0.");

        FilterBy([
            a => a.Started_Airing != null,
            a => a.Started_Airing <= DateTime.UtcNow,
        ]);
        SortBy([
            SortAction<Anime>.Desc(a => a.Started_Airing),
            SortAction<Anime>.Desc(a => a.Score)
        ]);
        Limit(count);

        return this;
    }

    public AnimeQuery ExcludeAdultContent(bool exclude)
    {
        if (exclude)
            FilterBy(a =>
                !string.IsNullOrEmpty(a.Rating) &&
                !a.Rating.ToLower().Contains(Constants.Ratings.AdultContent));

        return this;
    }
    public AnimeQuery ExcludeAdultContent()
        => ExcludeAdultContent(true);

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

    public AnimeQuery IncludeJunctions()
    {
        Include(q => q.Include(a => a.Anime_Genres));
        Include(q => q.Include(a => a.Anime_Licensors));
        Include(q => q.Include(a => a.Anime_Producers));

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
