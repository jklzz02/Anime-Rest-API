using System.Linq.Expressions;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Abstractions.DataAccess.Models;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using AnimeApi.Server.DataAccess.Repositories.QueryHelpers;

namespace AnimeApi.Server.DataAccess.Repositories;

/// <summary>
/// Represents a repository for managing and querying Anime data.
/// </summary>
/// <remarks>
/// This class provides the implementation of the <see cref="IAnimeRepository"/> interface
/// and serves as a mediator between the database and application logic,
/// enabling operations such as retrieval, addition, update, and deletion of <see cref="Anime"/> entities.
/// </remarks>
public class AnimeRepository : IAnimeRepository
{
    private readonly AnimeDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimeRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for anime data operations.</param>
    public AnimeRepository(AnimeDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Anime?> GetByIdAsync(int id)
    {
        return await GetByIdAsync(id, false);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetAllAsync()
    {
        var query = new AnimeQuery(_context.Anime)
            .AsNoTracking()
            .AsSplitQuery()
            .IncludeFullRelation()
            .ApplySorting(a => a.Id);
        
        return await query.Build().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetAllAsync(int page, int size = 100)
    {
        var paginationErrors = ValidatePageAndSize(page, size);

        if (paginationErrors.Any())
        {
            return new PaginatedResult<Anime>(paginationErrors);
        }

        var count = await _context.Anime.CountAsync();

        var query = new AnimeQuery(_context.Anime)
            .AsNoTracking()
            .AsSplitQuery()
            .IncludeFullRelation()
            .ApplySorting(a => a.Score, true)
            .ApplyPagination(page, size);
        
        var entities = await query.Build().ToListAsync();

        return new PaginatedResult<Anime>(entities, page, size, count);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetAllNonAdultAsync(int page, int size)
    {
        var paginationErrors = ValidatePageAndSize(page, size);

        if (paginationErrors.Any())
        {
            return new PaginatedResult<Anime>(paginationErrors);
        }

        var query = new AnimeQuery(_context.Anime)
            .AsNoTracking()
            .AsSplitQuery()
            .AsExpandable()
            .IncludeFullRelation()
            .ApplyFilters([
                a => !string.IsNullOrEmpty(a.Rating),
                a => !a.Rating.ToLower().Contains(Constants.Ratings.AdultContent),
            ])
            .ApplySorting(a => a.Score, true);

        var count = await query.Build().CountAsync();
        var entities = await query
            .ApplyPagination(page, size)
            .Build()
            .ToListAsync();

        return new PaginatedResult<Anime>(entities, page, size, count);
    }

    public async Task<IEnumerable<Anime>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await new AnimeQuery(_context.Anime)
            .AsNoTracking()
            .AsSplitQuery()
            .IncludeFullRelation()
            .ApplyFilter(a => ids.Contains(a.Id))
            .Build()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetMostRecentAsync(int count)
    {
        if (count <= 0)
        {
           return [];
        }

        return await _context.Anime
            .AsNoTracking()
            .AsSplitQuery()
            .Include(a => a.Anime_Genres)
            .Include(a => a.Anime_Producers)
            .Include(a => a.Anime_Licensors)
            .Include(a => a.Type)
            .Include(a => a.Source)
            .Include(a => a.Favourites)
            .Include(a => a.Reviews)
            .Where(a => a.Started_Airing != null && a.Started_Airing <= DateTime.UtcNow)
            .Where(a => !string.IsNullOrEmpty(a.Rating) && !a.Rating.Contains(Constants.Ratings.AdultContent))
            .OrderByDescending(a => a.Started_Airing)
            .ThenByDescending(a => a.Score)
            .Take(count)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Anime?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        return await new AnimeQuery(_context.Anime)
            .AsNoTracking()
            .AsSplitQuery()
            .IncludeFullRelation()
            .ApplyFilter(condition)
            .Build()
            .FirstOrDefaultAsync();
    }

    public async Task<PaginatedResult<Anime>> GetByConditionAsync(
        int page = 1,
        int size = 100,
        IEnumerable<Expression<Func<Anime, bool>>>? filters = null,
        Expression<Func<Anime, Object>>? orderBy = null,
        bool desc = true
        )
    {
        var paginationErrors = ValidatePageAndSize(page, size);
        
        if (paginationErrors.Any())
        {
            return new PaginatedResult<Anime>(paginationErrors);
        }

        var query = new AnimeQuery(_context.Anime)
            .AsExpandable()
            .AsSplitQuery()
            .AsNoTracking()
            .IncludeFullRelation();

        if (filters is not null)
        {
           query.ApplyFilters(filters);
        }

        if (orderBy is not null)
        {
          query.ApplySorting(orderBy, desc);
        }
        else
        {
          query.ApplySorting(a => a.Score, true);
        }

        var count = await query.Build().CountAsync();
        
        query.ApplyPagination(page, size);

        var resultQuery = query.Build();
        var entities = await 
            resultQuery.ToListAsync();

        return new PaginatedResult<Anime>(entities, page, size, count);
    }

    public async Task<PaginatedResult<Anime>> GetByParamsAsync(AnimeSearchParameters parameters, int page,
        int size = 100)
    {
        var paginationErrors = ValidatePageAndSize(page, size);

        if (paginationErrors.Any())
        {
            return new PaginatedResult<Anime>(paginationErrors);
        }
        
        var filters = BuildFilters(parameters);
        var orderBy = OrderByClause(parameters);

        var desc = !(parameters.SortOrder?.EqualsIgnoreCase(Constants.OrderBy.Directions.Ascending) ?? false); 

        var result = await GetByConditionAsync(page, size, filters, orderBy, desc);
        return result;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnimeSummary>> GetSummariesAsync(int count)
    {
        if (count <= 0)
        {
            return [];
        }

        var entities = await _context.Anime
            .AsNoTracking()
            .OrderByDescending(a => a.Score)
            .Take(count)
            .ToListAsync();

        return entities.Select(a => new AnimeSummary
        {
            Id = a.Id,
            Name = a.Name,
            ImageUrl = a.Image_URL,
            Score = a.Score,
            ReleaseYear = a.Release_Year,
            Rating = a.Rating
        });
    }
    
        /// <inheritdoc />
    public async Task<Result<Anime>> AddAsync(Anime entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var anime = await GetByIdAsync(entity.Id);
        if (anime is not null)
        {
            return Result<Anime>.ValidationFailure("id", $"Cannot add another anime with id '{entity.Id}'");
        }

        var foreignKeysErros = await 
            ValidateForeignKeysAsync(
                entity.Anime_Genres,
                entity.Anime_Producers,
                entity.Anime_Licensors,
                entity.TypeId,
                entity.SourceId ?? 0);

        if (foreignKeysErros.Any())
        {
            return Result<Anime>.Failure(foreignKeysErros);
        }

        _context.Anime.Add(entity);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            return Result<Anime>.InternalFailure("create", "something went wrong during entity creation.");
        }
        
        _context.ChangeTracker.Clear();
        var refreshedEntity = await 
            GetByIdAsync(entity.Id);

        return Result<Anime>.Success(refreshedEntity!);
    }

    /// <inheritdoc />
    public async Task<Result<Anime>> UpdateAsync(Anime entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var anime = await GetByIdAsync(entity.Id, true);
        if (anime is null)
        {
            return Result<Anime>.InternalFailure("update", $"there's no anime with id '{entity.Id}'.");
        }

        var foreignKeysErros = await
            ValidateForeignKeysAsync(
                entity.Anime_Genres,
                entity.Anime_Producers,
                entity.Anime_Licensors,
                entity.TypeId,
                entity.SourceId ?? 0);

        if (!foreignKeysErros.Any())
        {
            return Result<Anime>.Failure(foreignKeysErros);
        }

        UpdateAnime(anime, entity);
        await UpdateRelations(anime.Anime_Genres.ToList(), entity.Anime_Genres.ToList());
        await UpdateRelations(anime.Anime_Producers.ToList(), entity.Anime_Producers.ToList());
        await UpdateRelations(anime.Anime_Licensors.ToList(), entity.Anime_Licensors.ToList());

        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            return Result<Anime>.InternalFailure("update", "something went wrong during entity update.");
        }
        
        _context.ChangeTracker.Clear();
        var refreshedEntity = await 
            GetByIdAsync(entity.Id);

        return Result<Anime>.Success(refreshedEntity!);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        var anime = await GetByIdAsync(id);
        if (anime is null) return false;

        _context.Anime.Remove(anime);
        return await _context.SaveChangesAsync() > 0;
    }

    private async Task<Anime?> GetByIdAsync(int id, bool trackEntity)
    {
        var query = new AnimeQuery(_context.Anime)
            .IncludeFullRelation()
            .AsSplitQuery()
            .ApplyFilter(a => a.Id == id);

        if (!trackEntity)
        {
            query = query.AsNoTracking();
        }

        return await query.Build().FirstOrDefaultAsync();
    }

    private List<Error> ValidatePageAndSize(int page, int size)
    {
        List<Error> errors = [];

        if (page <= 0)
        {
            errors.Add(Error.Validation("page", "must be greater than 0."));
        }

        if (size < Constants.Pagination.MinPageSize)
        {
            errors.Add(Error.Validation("size", $"must be at least {Constants.Pagination.MinPageSize}."));
        }

        if (size > Constants.Pagination.MaxPageSize)
        {
            errors.Add(Error.Validation("size", $"cannot be greater than {Constants.Pagination.MaxPageSize}."));
        }

        return errors;
    }

    private async Task<List<Error>> ValidateForeignKeysAsync(
        ICollection<AnimeGenre> genres,
        ICollection<AnimeProducer> producers,
        ICollection<AnimeLicensor> licensors,
        int typeId,
        int sourceId)
    {
        List<Error> Errors = new();

        var genresIds = genres.Select(ag => ag.GenreId).ToList();
        var producersIds = producers.Select(ap => ap.ProducerId).ToList();
        var licensorsIds = licensors.Select(al => al.LicensorId).ToList();

        var genresExistingIds = await _context.Genres
            .AsNoTracking()
            .Select(g => g.Id)
            .ToListAsync();

        var producersExistingIds = await _context.Producers
            .AsNoTracking()
            .Select(p => p.Id)
            .ToListAsync();

        var licensorsExistingIds = await _context.Licensors
            .AsNoTracking()
            .Select(l => l.Id)
            .ToListAsync();

        var typesExistingIds = await _context.Types
            .AsNoTracking()
            .Select(t => t.Id)
            .ToListAsync();

        var sourcesExistingIds = await _context.Sources
            .AsNoTracking()
            .Select(s => s.Id)
            .ToListAsync();

        if (!genresIds.All(g => genresExistingIds.Contains(g)))
        {
            Errors.Add(Error.Validation("genres", "on or more genre entities ids do not exist."));
        }


        if (!licensorsIds.All(l => licensorsExistingIds.Contains(l)))
        {
            Errors.Add(Error.Validation("licensors", "one or more licensor entities ids do not exist."));
        }


        if (!producersIds.All(g => producersExistingIds.Contains(g)))
        {
            Errors.Add(Error.Validation("producers", "one or more producer entities ids do not exist."));
        }

        if (!typesExistingIds.Contains(typeId))
        {
            Errors.Add(Error.Validation("types", $"there's no anime type with id {typeId}"));
        }

        if (!sourcesExistingIds.Contains(sourceId))
        {
            Errors.Add(Error.Validation("sources", $"there's no anime source with id {sourceId}"));
        }

        return Errors;
    }

    private Expression<Func<Anime, object>> OrderByClause(AnimeSearchParameters parameters)
    {
        Dictionary<string, Expression<Func<Anime, object>>> orderByMap = new()
        {
            {Constants.OrderBy.Fields.Id, a => a.Id},
            {Constants.OrderBy.Fields.Name, a => a.Name},
            {Constants.OrderBy.Fields.ReleaseYear, a => a.Release_Year},
            {Constants.OrderBy.Fields.ReleaseDate, a => a.Started_Airing},
            {Constants.OrderBy.Fields.Score, a => a.Score}
        };

        if (string.IsNullOrWhiteSpace(parameters.OrderBy))
        {
            return orderByMap[Constants.OrderBy.Fields.Score];
        }
        
        return orderByMap[parameters.OrderBy.Trim().ToLowerInvariant()];
    }

    private IEnumerable<Expression<Func<Anime, bool>>> BuildFilters(AnimeSearchParameters parameters)
    {
        var filters = new AnimeFilterBuilder()
            .WithFullTextSearch(parameters.Query)
            .WithName(parameters.Name)
            .WithEnglishName(parameters.EnglishName)
            .WithSource(parameters.Source)
            .WithType(parameters.Type)
            .WithGenres(parameters.GenreId, parameters.GenreName, parameters.GenreNames)
            .WithProducers(parameters.ProducerId, parameters.ProducerName, parameters.ProducerNames)
            .WithLicensors(parameters.LicensorId, parameters.LicensorName, parameters.LicensorNames)
            .WithStatus(parameters.Status)
            .WithStudio(parameters.Studio)
            .ExcludeAdultContent(!parameters.IncludeAdultContext)
            .WithScoreRange(parameters.MinScore, parameters.MaxScore)
            .WithYearRange(parameters.MinReleaseYear, parameters.MaxReleaseYear)
            .WithEpisodeRange(parameters.MinEpisodes, parameters.MaxEpisodes, parameters.Episodes)
            .WithAirDateRange(parameters.StartDateFrom, parameters.StartDateTo, parameters.EndDateFrom, parameters.EndDateTo);

        return filters.Build();
    }

    private void UpdateAnime(Anime original, Anime updated)
    {
        original.Name = updated.Name;
        original.English_Name = updated.English_Name;
        original.Other_Name = updated.Other_Name;
        original.Synopsis = updated.Synopsis;
        original.Image_URL = updated.Image_URL;
        original.TypeId = updated.TypeId;
        original.SourceId = updated.SourceId;
        original.Episodes = updated.Episodes;
        original.Duration = updated.Duration;
        original.SourceId = updated.SourceId;
        original.Release_Year = updated.Release_Year;
        original.Started_Airing = updated.Started_Airing;
        original.Finished_Airing = updated.Finished_Airing;
        original.Rating = updated.Rating;
        original.Studio = updated.Studio;
        original.Score = updated.Score;
        original.Status = updated.Status;
    }

   private async Task UpdateRelations<T>(
    List<T> original,
    List<T> updated)
    where T : class, IAnimeRelation, new()
    {
        var originalIds = original.Select(a => a.RelatedId).ToList();
        var updatedIds = updated.Select(a => a.RelatedId).ToList();
        
        if (originalIds.SequenceEqual(updatedIds))
        {
            return;
        }
        
        var animeId = original.FirstOrDefault()?.AnimeId ?? updated.FirstOrDefault()?.AnimeId ?? 0;
        
        var toRemove = original.Where(o => !updatedIds.Contains(o.RelatedId)).ToList();
        
        toRemove.ForEach(x => _context.Set<T>().Remove(x));
        
        var idsToAdd = updatedIds.Where(id => !originalIds.Contains(id)).ToList();

        var newRelations = idsToAdd
            .Select(id => new T { AnimeId = animeId, RelatedId = id });
            
        await _context.Set<T>().AddRangeAsync(newRelations);
    }
}