
using System.Linq.Expressions;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Objects.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories.QueryHelpers;

public class AnimeFilterBuilder
{
    private readonly List<Expression<Func<Anime, bool>>> _filters = new();

    public AnimeFilterBuilder WithFullTextSearch(string? query)
    {
        if (!string.IsNullOrWhiteSpace(query))
        {
            _filters.Add(a =>
                EF.Functions.TrigramsAreSimilar(query, a.Name) ||
                EF.Functions.TrigramsAreSimilar(query, a.English_Name));
        }

        return this;
    }

    public AnimeFilterBuilder WithName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            _filters.Add(a => a.Name.Contains(name));
        
        return this;
    }

    public AnimeFilterBuilder WithEnglishName(string? englishName)
    {
        if (!string.IsNullOrWhiteSpace(englishName))
            _filters.Add(a => a.English_Name.Contains(englishName));
        
        return this;
    }

    public AnimeFilterBuilder WithSource(int? sourceId)
        => WithSource(sourceId ,null);

    public AnimeFilterBuilder WithSource(string? source)
        => WithSource(null, source);

    public AnimeFilterBuilder WithSource(int? sourceId, string? source)
    {
        if (!string.IsNullOrWhiteSpace(source))
            _filters.Add(a => a.Source.Name.ToLower().Contains(source.ToLower()));

        if (sourceId.HasValue)
            _filters.Add(a => a.SourceId == sourceId);

        return this;
    }

    public AnimeFilterBuilder WithType(int? typeId)
        => WithType(typeId, null);

    public AnimeFilterBuilder WithType(string? type)
        => WithType(null, type);

    public AnimeFilterBuilder WithType(int? typeId, string? type)
    {
        if (!string.IsNullOrWhiteSpace(type))
            _filters.Add(a => a.Type.Name.ToLower().Contains(type.ToLower()));

        if (typeId.HasValue)
            _filters.Add(a => a.TypeId == typeId);

        return this;
    }

    public AnimeFilterBuilder WithStatus(string? status)
    {
        if (!string.IsNullOrWhiteSpace(status))
            _filters.Add(a => a.Status.Contains(status));

        return this;
    }

    public AnimeFilterBuilder WithStudio(string? studio)
    {
        if (!string.IsNullOrWhiteSpace(studio))
            _filters.Add(a => a.Studio.ToLower() == studio.ToLower());
        
        return this;
    }

    public AnimeFilterBuilder WithScoreRange(decimal? minScore, decimal? maxScore)
    {
        if (minScore.HasValue)
            _filters.Add(a => a.Score >= minScore);
        if (maxScore.HasValue)
            _filters.Add(a => a.Score <= maxScore);

        return this;
    }

    public AnimeFilterBuilder WithEpisodeRange(int? minEpisodes, int? maxEpisodes, int? exactEpisodes = null)
    {
        if (exactEpisodes.HasValue)
        {
            _filters.Add(a => a.Episodes == exactEpisodes);
        }
        else
        {
            if (minEpisodes.HasValue)
                _filters.Add(a => a.Episodes >= minEpisodes);
            if (maxEpisodes.HasValue)
                _filters.Add(a => a.Episodes <= maxEpisodes);
        }
        
        return this;
    }

    public AnimeFilterBuilder WithYearRange(int? minYear, int? maxYear)
    {
        if (minYear.HasValue)
            _filters.Add(a => a.Release_Year >= minYear);
        if (maxYear.HasValue)
            _filters.Add(a => a.Release_Year <= maxYear && a.Release_Year != 0);

        return this;
    }

    public AnimeFilterBuilder WithAirDateRange(
        DateTime? startFrom,
        DateTime? startTo,
        DateTime? endFrom,
        DateTime? endTo)
    {
        if (startFrom.HasValue)
            _filters.Add(a => a.Started_Airing >= startFrom.Value.ToUniversalTime());
        if (startTo.HasValue)
            _filters.Add(a => a.Started_Airing <= startTo.Value.ToUniversalTime());
        if (endFrom.HasValue)
            _filters.Add(a => a.Finished_Airing >= endFrom.Value.ToUniversalTime());
        if (endTo.HasValue)
            _filters.Add(a => a.Finished_Airing <= endTo.Value.ToUniversalTime());

        return this;
    }

    public AnimeFilterBuilder WithGenres(
        int? genreId = null,
        string? genreName = null,
        IEnumerable<string>? genreNames = null)
    {
        if (genreId.HasValue)
            _filters.Add(a => a.Anime_Genres.Any(g => g.GenreId == genreId));

        if (!string.IsNullOrWhiteSpace(genreName))
            _filters.Add(a => a.Anime_Genres.Any(g => g.Genre.Name.ToLower().Contains(genreName.ToLower())));

        if (genreNames?.Any() ?? false)
            _filters.Add(a => genreNames.All(g => a.Anime_Genres.Any(ag => ag.Genre.Name.ToLower() == g.ToLower())));

        return this;
    }

    public AnimeFilterBuilder WithProducers(
        int? producerId = null,
        string? producerName = null,
        IEnumerable<string>? producerNames = null)
    {
        if (producerId.HasValue)
            _filters.Add(a => a.Anime_Producers.Any(p => p.ProducerId == producerId));

        if (!string.IsNullOrWhiteSpace(producerName))
            _filters.Add(a => a.Anime_Producers.Any(p => p.Producer.Name.ToLower().Contains(producerName.ToLower())));

        if (producerNames?.Any() ?? false)
            _filters.Add(a => producerNames.All(p => a.Anime_Producers.Any(ap => ap.Producer.Name.ToLower() == p.ToLower())));

        return this;
    }

    public AnimeFilterBuilder WithLicensors(
        int? licensorId = null,
        string? licensorName = null,
        IEnumerable<string>? licensorNames = null)
    {
        if (licensorId.HasValue)
            _filters.Add(a => a.Anime_Licensors.Any(l => l.LicensorId == licensorId));

        if (!string.IsNullOrWhiteSpace(licensorName))
            _filters.Add(a => a.Anime_Licensors.Any(l => l.Licensor.Name.ToLower().Contains(licensorName.ToLower())));

        if (licensorNames?.Any() ?? false)
            _filters.Add(a => licensorNames.All(l => a.Anime_Licensors.Any(al => al.Licensor.Name.ToLower() == l.ToLower())));

        return this;
    }

    public AnimeFilterBuilder ExcludeAdultContent(bool exclude)
    {
        if (exclude)
            _filters.Add(a =>
            !string.IsNullOrEmpty(a.Rating) &&
            !a.Rating.ToLower().Contains(Constants.Ratings.AdultContent));
       
        return this;
    }
    public AnimeFilterBuilder ExcludeAdultContent()
        => ExcludeAdultContent(true);

    public IEnumerable<Expression<Func<Anime, bool>>> Build() 
        => _filters;
}
