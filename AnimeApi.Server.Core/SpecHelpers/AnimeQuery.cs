using AnimeApi.Server.Core.Abstractions.DataAccess.Specification;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.Core.SpecHelpers;

public class AnimeQuery : QuerySpec<Anime, AnimeQuery>
{

    public AnimeQuery ByPk(int id)
        => FilterBy(a => a.Id == id);

    public AnimeQuery ByPk(IEnumerable<int> id)
        =>  FilterBy(a => id.Contains(a.Id));

    public AnimeQuery WithFullTextSearch(string? query)
    {
        if (!string.IsNullOrWhiteSpace(query))
        {
            FilterBy(a =>
                EF.Functions.TrigramsAreSimilar(query, a.Name) ||
                EF.Functions.TrigramsAreSimilar(query, a.EnglishName));
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
            FilterBy(a => a.EnglishName.Contains(englishName));

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

            return this;
        }

        if (minEpisodes.HasValue)
            FilterBy(a => a.Episodes >= minEpisodes);

        if (maxEpisodes.HasValue)
            FilterBy(a => a.Episodes <= maxEpisodes);

        return this;
    }

    public AnimeQuery WithYearRange(int? minYear, int? maxYear)
    {
        if (minYear.HasValue)
            FilterBy(a => a.ReleaseYear >= minYear);
        
        if (maxYear.HasValue)
            FilterBy(a => a.ReleaseYear <= maxYear && a.ReleaseYear != 0);

        return this;
    }

    public AnimeQuery WithAirDateRange(
        DateTime? startFrom,
        DateTime? startTo,
        DateTime? endFrom,
        DateTime? endTo)
    {
        if (startFrom.HasValue)
            FilterBy(a => a.StartedAiring >= startFrom.Value.ToUniversalTime());

        if (startTo.HasValue)
            FilterBy(a => a.StartedAiring <= startTo.Value.ToUniversalTime());
        
        if (endFrom.HasValue)
            FilterBy(a => a.FinishedAiring >= endFrom.Value.ToUniversalTime());
        
        if (endTo.HasValue)
            FilterBy(a => a.FinishedAiring <= endTo.Value.ToUniversalTime());

        return this;
    }

    public AnimeQuery WithGenres(
        int? genreId = null,
        string? genreName = null,
        IEnumerable<string>? genreNames = null)
    {
        if (genreId.HasValue)
            FilterBy(a => a.AnimeGenres.Any(g => g.GenreId == genreId));

        if (!string.IsNullOrWhiteSpace(genreName))
            FilterBy(a => a.AnimeGenres.Any(g => g.Genre.Name.ToLower().Contains(genreName.ToLower())));

        if (genreNames?.Any() ?? false)
            FilterBy(a => genreNames.All(g => a.AnimeGenres.Any(ag => ag.Genre.Name.ToLower() == g.ToLower())));

        return this;
    }

    public AnimeQuery WithProducers(
        int? producerId = null,
        string? producerName = null,
        IEnumerable<string>? producerNames = null)
    {
        if (producerId.HasValue)
            FilterBy(a => a.AnimeProducers.Any(p => p.ProducerId == producerId));

        if (!string.IsNullOrWhiteSpace(producerName))
            FilterBy(a => a.AnimeProducers.Any(p => p.Producer.Name.ToLower().Contains(producerName.ToLower())));

        if (producerNames?.Any() ?? false)
            FilterBy(a => producerNames.All(p => a.AnimeProducers.Any(ap => ap.Producer.Name.ToLower() == p.ToLower())));

        return this;
    }

    public AnimeQuery WithLicensors(
        int? licensorId = null,
        string? licensorName = null,
        IEnumerable<string>? licensorNames = null)
    {
        if (licensorId.HasValue)
            FilterBy(a => a.AnimeLicensors.Any(l => l.LicensorId == licensorId));

        if (!string.IsNullOrWhiteSpace(licensorName))
            FilterBy(a => a.AnimeLicensors.Any(l => l.Licensor.Name.ToLower().Contains(licensorName.ToLower())));

        if (licensorNames?.Any() ?? false)
            FilterBy(a => licensorNames.All(l => a.AnimeLicensors.Any(al => al.Licensor.Name.ToLower() == l.ToLower())));

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
                ? SortAction<Anime>.Asc(a => a.ReleaseYear)
                : SortAction<Anime>.Desc(a => a.ReleaseYear),

            Constants.OrderBy.Fields.ReleaseDate => ascending
                ? SortAction<Anime>.Asc(a => a.StartedAiring)
                : SortAction<Anime>.Desc(a => a.StartedAiring),

            Constants.OrderBy.Fields.Episodes => ascending
                ? SortAction<Anime>.Asc(a => a.Episodes)
                : SortAction<Anime>.Desc(a => a.Episodes),

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
            a => a.StartedAiring != null,
            a => a.StartedAiring <= DateTime.UtcNow,
        ]);
        SortBy([
            SortAction<Anime>.Desc(a => a.StartedAiring),
            SortAction<Anime>.Desc(a => a.Score),
            SortAction<Anime>.Asc(a => a.Id)
        ]);
        Limit(count);

        return this;
    }

    public AnimeQuery ExcludeAdultContent(bool exclude)
    {
        if (exclude)
            FilterBy([
                a => !string.IsNullOrEmpty(a.Rating),
                a => a.Rating.Trim() != string.Empty,
                a => !a.Rating.Trim().StartsWith(Constants.Ratings.AdultContent),
                a => a.AnimeGenres.All(ag => ag.Genre.Name.Trim().ToLower() != "hentai")
            ]);

        return this;
    }
    
    public AnimeQuery ExcludeAdultContent()
        => ExcludeAdultContent(true);

    public AnimeQuery IncludeProducers()
    {
        Include(q => q.Include(a => a.AnimeProducers)
            .ThenInclude(ap => ap.Producer));

        return this;
    }

    public AnimeQuery IncludeLicensors()
    {
        Include(q => q.Include(a => a.AnimeLicensors)
                .ThenInclude(al => al.Licensor));
        
        return this;
    }

    public AnimeQuery IncludeGenres()
    {
        Include(q => q.Include(a => a.AnimeGenres)
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
        Include(q => q.Include(a => a.AnimeGenres));
        Include(q => q.Include(a => a.AnimeLicensors));
        Include(q => q.Include(a => a.AnimeProducers));

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
