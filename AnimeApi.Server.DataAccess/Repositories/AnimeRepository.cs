using System.Linq.Expressions;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Extensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

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
    public Dictionary<string, string> ErrorMessages { get; } = new();
    
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
                .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetAllAsync()
    {
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
            .OrderBy(a => a.Score)
            .ThenByDescending(a => a.Release_Year)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetAllAsync(int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        var count = await _context.Anime.CountAsync();

        var entities = await _context.Anime
            .AsNoTracking()
            .AsSplitQuery()
            .Include(a => a.Anime_Genres)
            .Include(a => a.Anime_Producers)
            .Include(a => a.Anime_Licensors)
            .Include(a => a.Type)
            .Include(a => a.Source)
            .OrderByDescending(a => a.Score)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        
        return new PaginatedResult<Anime>(entities, page, size, count);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetAllNonAdultAsync(int page, int size)
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }

        var query = _context.Anime
            .AsNoTracking()
            .AsSplitQuery()
            .Include(a => a.Anime_Genres)
            .Include(a => a.Anime_Producers)
            .Include(a => a.Anime_Licensors)
            .Include(a => a.Type)
            .Include(a => a.Source)
            .Where(a => !string.IsNullOrEmpty(a.Rating) && !a.Rating.Contains(Constants.Ratings.AdultContent))
            .OrderByDescending(a => a.Score);
            
        
        var entities = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        
        return new PaginatedResult<Anime>(entities, page, size, await query.CountAsync());
    }

    public async Task<IEnumerable<Anime>> GetByIdsAsync(IEnumerable<int> ids)
    {
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
            .Where(a => ids.Contains(a.Id))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Anime?> AddAsync(Anime entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var anime = await GetByIdAsync(entity.Id);
        if (anime is not null)
        {
            ErrorMessages.Add("id", $"Cannot add another anime with id '{entity.Id}'");
            return null;
        }

        var areForeignKeysValid = await ValidateForeignKeysAsync(
            entity.Anime_Genres,
            entity.Anime_Producers,
            entity.Anime_Licensors,
            entity.TypeId,
            entity.SourceId ?? 0);
        
        if(!areForeignKeysValid)
        {
            return null;
        }
        
        _context.Anime.Add(entity);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            return null;
        }
        
        _context.Entry(entity).State = EntityState.Detached;
        
        entity.Anime_Genres
            .Select(ag => ag.Genre)
            .ForEach(g => _context.Entry(g).State = EntityState.Detached);
        
        entity.Anime_Producers
            .Select(ap => ap.Producer)
            .ForEach(p => _context.Entry(p).State = EntityState.Detached);
        
        entity.Anime_Licensors
            .Select(al => al.Licensor)
            .ForEach(l => _context.Entry(l).State = EntityState.Detached);
        
        return await GetByIdAsync(entity.Id);
    }

    /// <inheritdoc />
    public async Task<Anime?> UpdateAsync(Anime entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var anime = await GetByIdAsync(entity.Id);
        if (anime is null)
        {
            ErrorMessages.Add("id", $"There is no anime with id {entity.Id}");
            return null;
        }

        var areForeignKeysValid = await ValidateForeignKeysAsync(
            entity.Anime_Genres,
            entity.Anime_Producers,
            entity.Anime_Licensors,
            entity.TypeId,
            entity.SourceId ?? 0);
        
        if(!areForeignKeysValid)
        {
            return null;
        }
        
        UpdateAnime(anime, entity);
        _context.Update(anime);
        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result)
        {
            return null;
        }
        
        _context.Entry(anime).State = EntityState.Detached;
        
        anime.Anime_Genres
            .Select(ag => ag.Genre)
            .ForEach(g => _context.Entry(g).State = EntityState.Detached);
        
        anime.Anime_Producers
            .Select(ap => ap.Producer)
            .ForEach(p => _context.Entry(p).State = EntityState.Detached);
        
        anime.Anime_Licensors
            .Select(al => al.Licensor)
            .ForEach(l => _context.Entry(l).State = EntityState.Detached);
        
        return await GetByIdAsync(anime.Id);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        var anime = await GetByIdAsync(id);
        if (anime is null) return false;
        
        _context.Anime.Remove(anime);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByNameAsync(string name, int page, int size = 100)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(
            page,
            size,
            [a => EF.Functions.Like(a.Name, $"%{name}%")]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByEnglishNameAsync(string englishName, int page, int size = 100)
    {
        ArgumentNullException.ThrowIfNull(englishName, nameof(englishName));
        ArgumentException.ThrowIfNullOrEmpty(englishName, nameof(englishName));
        
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(
            page,
            size,
            [a => EF.Functions.Like(a.English_Name, $"%{englishName}%")]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetBySourceAsync(string source, int page, int size = 100)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentException.ThrowIfNullOrEmpty(source, nameof(source));
        
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(
            page,
            size,
            [a => EF.Functions.Like(a.Source, $"%{source}")]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByTypeAsync(string type, int page, int size = 100)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        ArgumentException.ThrowIfNullOrEmpty(type, nameof(type));
        
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(
            page,
            size,
            [a => EF.Functions.Like(a.Type.Name, $"%{type}")]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByScoreAsync(int score, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync( page, size, [a => a.Score == score]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByLicensorAsync(int licensorId, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(
            page,
            size,
            [a => a.Anime_Licensors.Any(al => al.LicensorId == licensorId)]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByProducerAsync(int producerId, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(
            page,
            size,
            [a => a.Anime_Producers.Any(ap => ap.ProducerId == producerId)]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByGenreAsync(int genreId, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(
            page,
            size,
            [a => a.Anime_Genres.Any(ag => ag.GenreId == genreId)]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByReleaseYearAsync(int year, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(page, size, [a => a.Release_Year == year]);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<Anime>> GetByEpisodesAsync(int episodes, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        return await GetByConditionAsync(page, size,[a => a.Episodes == episodes]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetMostRecentAsync(int count)
    {
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
        var anime = await _context.Anime
            .AsSplitQuery()
            .AsNoTracking()
            .Include(a => a.Anime_Genres)
            .Include(a => a.Anime_Producers)
            .Include(a => a.Anime_Licensors)
            .Include(a => a.Type)
            .Include(a => a.Source)
            .Include(a => a.Favourites)
            .Include(a => a.Reviews)
            .FirstOrDefaultAsync(condition);
        
        return anime;
    }
    public async Task<PaginatedResult<Anime>> GetByConditionAsync(
        int page = 1,
        int size = 100,
        IEnumerable<Expression<Func<Anime, bool>>>? filters = null)    
    {
        if (!ValidatePageAndSize(page, size))
        {
            return new PaginatedResult<Anime>(new List<Anime>(), page, size);
        }
        
        var query = _context.Anime
            .AsExpandable();

        if (filters is not null)
        {
            query = filters
                .Aggregate(query, (current, filter) => current.Where(filter));
        }

        var ids = await query
            .AsSplitQuery()
            .AsNoTracking()
            .OrderByDescending(a => a.Score)
            .Select(a => a.Id)
            .ToListAsync();
        
        var paginatedIds = ids
            .Skip((page - 1) * size)
            .Take(size);
        
        var entities = await _context.Anime
            .AsSplitQuery()
            .AsNoTracking()
            .Include(a => a.Anime_Genres)
            .Include(a => a.Anime_Producers)
            .Include(a => a.Anime_Licensors)
            .Include(a => a.Type)
            .Include(a => a.Source)
            .Include(a => a.Favourites)
            .Include(a => a.Reviews)
            .Where(a => paginatedIds.Contains(a.Id))
            .ToListAsync();

        return new PaginatedResult<Anime>(entities, page, size, await query.CountAsync());
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AnimeSummary>> GetSummariesAsync(int count)
    {       
        if (count <= 0)
        {
            throw new ArgumentException($"{nameof(count)} must be greater than 0");
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

    private bool ValidatePageAndSize(int page, int size)
    {
        if (page <= 0)
        {
            ErrorMessages.Add("page", "must be greater than 0");
        }

        if (size < Constants.Pagination.MinPageSize)
        {
            ErrorMessages.Add("page", $"must be greater than or equal to {Constants.Pagination.MinPageSize}");
        }

        if (size > Constants.Pagination.MaxPageSize)
        {
            ErrorMessages.Add("page", $"must be less than or equal to {Constants.Pagination.MaxPageSize}");
        }

        return !ErrorMessages.Any();
    }
    
    private async Task<bool>  ValidateForeignKeysAsync(
        ICollection<AnimeGenre> genres,
        ICollection<AnimeProducer> producers,
        ICollection<AnimeLicensor> licensors,
        int typeId,
        int sourceId)
    {
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
            ErrorMessages.Add("genres", "on or more genre entities ids do not exist.");
        }
        
        
        if (!licensorsIds.All(l => licensorsExistingIds.Contains(l)))
        {
            ErrorMessages.Add("licensors", "one or more licensor entities ids do not exist.");
        }
        
        
        if (!producersIds.All(g => producersExistingIds.Contains(g)))
        {
            ErrorMessages.Add("producers", "one or more producer entities ids do not exist.");
        }

        if (!typesExistingIds.Contains(typeId))
        {
            ErrorMessages.Add("types", $"there's no anime type with id {typeId}");
        }

        if (!sourcesExistingIds.Contains(sourceId))
        {
            ErrorMessages.Add("sources", $"there's no anime source with id {sourceId}");
        }

        return !ErrorMessages.Any();
    }

    private void UpdateAnime(Anime original, Anime updated)
    {
        original.Name = updated.Name;
        original.English_Name = updated.English_Name;
        original.Other_Name = updated.Other_Name;
        original.Synopsis = updated.Synopsis;
        original.Image_URL = updated.Image_URL;
        original.TypeId = updated.TypeId;
        original.Type = updated.Type;
        original.SourceId = updated.SourceId;
        original.Source = updated.Source;
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
        original.Anime_Genres
            .Distinct()
            .ToList()
            .Update(updated.Anime_Genres, original.Id);
        original.Anime_Producers
            .Distinct()
            .ToList()
            .Update(updated.Anime_Producers, original.Id);
        original.Anime_Licensors
            .Update(updated.Anime_Licensors, original.Id);
    }
}