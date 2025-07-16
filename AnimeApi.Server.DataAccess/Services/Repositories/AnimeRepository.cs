using System.Linq.Expressions;
using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Extensions;
using AnimeApi.Server.DataAccess.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

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
                .FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetAllAsync()
    {
        return await _context.Anime
            .AsNoTracking()
            .AsSplitQuery()
            .OrderBy(a => a.Id)
            .ToListAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetAllAsync(int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size)) return [];
        
        var entities = await GetAllAsync();
        return entities
            .Skip((page - 1) * size)
            .Take(size);
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
    public async Task<IEnumerable<Anime>> GetByNameAsync(string name, int page, int size = 100)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        return await GetByConditionAsync(
            page,
            size,
            [a => EF.Functions.Like(a.Name, $"%{name}%")]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetByEnglishNameAsync(string englishName, int page, int size = 100)
    {
        ArgumentNullException.ThrowIfNull(englishName, nameof(englishName));
        ArgumentException.ThrowIfNullOrEmpty(englishName, nameof(englishName));
        
        return await GetByConditionAsync(
            page,
            size,
            [a => EF.Functions.Like(a.English_Name, $"%{englishName}%")]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetBySourceAsync(string source, int page, int size = 100)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentException.ThrowIfNullOrEmpty(source, nameof(source));
        
        return await GetByConditionAsync(
            page,
            size,
            [a => EF.Functions.Like(a.Source, $"%{source}")]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetByTypeAsync(string type, int page, int size = 100)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        ArgumentException.ThrowIfNullOrEmpty(type, nameof(type));
        
        return await GetByConditionAsync(
            page,
            size,
            [a => EF.Functions.Like(a.Type.Name, $"%{type}")]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetByScoreAsync(int score, int page, int size = 100)
    {
        return await GetByConditionAsync( page, size, [a => a.Score == score]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetByLicensorAsync(int licensorId, int page, int size = 100)
    {
        return await GetByConditionAsync(
            page,
            size,
            [a => a.Anime_Licensors.Any(al => al.LicensorId == licensorId)]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetByProducerAsync(int producerId, int page, int size = 100)
    {
        return await GetByConditionAsync(
            page,
            size,
            [a => a.Anime_Producers.Any(ap => ap.ProducerId == producerId)]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetByGenreAsync(int genreId, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size)) return [];
        
        return await GetByConditionAsync(
            page,
            size,
            [a => a.Anime_Genres.Any(ag => ag.GenreId == genreId)]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetByReleaseYearAsync(int year, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size)) return [];
        return await GetByConditionAsync(page, size, [a => a.Release_Year == year]);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Anime>> GetByEpisodesAsync(int episodes, int page, int size = 100)
    {
        if (!ValidatePageAndSize(page, size)) return [];
        return await GetByConditionAsync(page, size,[a => a.Episodes == episodes]);
    }
    
    /// <inheritdoc />
    public async Task<Anime?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        var anime = await _context.Anime
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(condition);

        return anime;
    }
    public async Task<IEnumerable<Anime>> GetByConditionAsync(
        int page = 1,
        int size = 100,
        IEnumerable<Expression<Func<Anime, bool>>>? filters = null)    
    {
        if (!ValidatePageAndSize(page, size)) return [];
        
        var query = _context.Anime
            .AsExpandable();

        if (filters is not null)
        {
            query = filters
                .Aggregate(query, (current, filter) => current.Where(filter));
        }

        var result = await query
            .OrderBy(a => a.Id)
            .AsNoTracking()
            .AsSplitQuery()
            .Skip((page -1) * size)
            .Take(size)
            .ToListAsync();

        return result;
    }

    private bool ValidatePageAndSize(int page, int size)
    {
        if (page <= 0)
        {
            ErrorMessages.Add("page", "must be greater than 0");
        }

        if (size <= 0)
        {
            ErrorMessages.Add("size", "must be greater than 0");
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