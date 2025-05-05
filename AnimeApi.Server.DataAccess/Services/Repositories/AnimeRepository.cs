using System.Linq.Expressions;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Extensions;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class AnimeRepository : IAnimeRepository
{
    private readonly AnimeDbContext _context;
    public Dictionary<string, string> ErrorMessages { get; } = new();

    public AnimeRepository(AnimeDbContext context)
    {
        _context = context;
    }

    public async Task<Anime?> GetByIdAsync(int id)
    {
        return await _context.Anime
                .AsSplitQuery()
                .Include(a => a.Anime_Genres)
                .ThenInclude(ag => ag.Genre)
                .Include(a => a.Anime_Licensors)
                .ThenInclude(al => al.Licensor)
                .Include(a => a.Anime_Producers)
                .ThenInclude(ap => ap.Producer)
                .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Anime>> GetAllAsync()
    {
        return await _context.Anime
            .OrderBy(a => a.Id)
            .Take(100)
            .ToListAsync();
    }

    public async Task<bool> AddAsync(Anime entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var anime = await GetByIdAsync(entity.Id);
        if (anime is not null)
        {
            ErrorMessages.Add("id", "There is already an anime with this id");
            return false;
        }
        
        _context.Anime.Add(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Anime entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var anime = await GetByIdAsync(entity.Id);
        if (anime is null)
        {
            ErrorMessages.Add("id", "There is no anime with this id");
            return false;
        }
        
        UpdateAnime(anime, entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var anime = await GetByIdAsync(id);
        if (anime is null) return false;
        
        _context.Anime.Remove(anime);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Anime>> GetByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        return await GetByConditionAsync([a => EF.Functions.Like(a.Name, $"%{name}%")]);
    }

    public async Task<IEnumerable<Anime>> GetByEnglishNameAsync(string englishName)
    {
        ArgumentNullException.ThrowIfNull(englishName, nameof(englishName));
        ArgumentException.ThrowIfNullOrEmpty(englishName, nameof(englishName));
        
        return await GetByConditionAsync([a => EF.Functions.Like(a.English_Name, $"%{englishName}%")]);
    }

    public async Task<IEnumerable<Anime>> GetBySourceAsync(string source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentException.ThrowIfNullOrEmpty(source, nameof(source));
        
        return await GetByConditionAsync([a => EF.Functions.Like(a.Source, $"%{source}")]);
    }

    public async Task<IEnumerable<Anime>> GetByTypeAsync(string type)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        ArgumentException.ThrowIfNullOrEmpty(type, nameof(type));
        
        return await GetByConditionAsync([a => EF.Functions.Like(a.Type, $"%{type}")]);
    }

    public async Task<IEnumerable<Anime>> GetByScoreAsync(int score)
    {
        return await GetByConditionAsync( [a => a.Score == score]);
    }

    public async Task<IEnumerable<Anime>> GetByLicensorAsync(int licensorId)
    {
        return await GetByConditionAsync([a => a.Anime_Licensors.Any(al => al.LicensorId == licensorId)]);
    }

    public async Task<IEnumerable<Anime>> GetByProducerAsync(int producerId)
    {
        return await GetByConditionAsync([a => a.Anime_Producers.Any(ap => ap.ProducerId == producerId)]);
    }

    public async Task<IEnumerable<Anime>> GetByGenreAsync(int genreId)
    {
        return await GetByConditionAsync([a => a.Anime_Genres.Any(ag => ag.GenreId == genreId)]);
    }

    public async Task<IEnumerable<Anime>> GetByReleaseYearAsync(int year)
    {
        return await GetByConditionAsync([a => a.Release_Year == year]);
    }

    public async Task<IEnumerable<Anime>> GetByEpisodesAsync(int episodes)
    {
        return await GetByConditionAsync([a => a.Episodes == episodes]);
    }
    
    public async Task<Anime?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        var anime = await _context.Anime
            .Include(a => a.Anime_Genres)
                .ThenInclude(ag => ag.Genre)
            .Include(a => a.Anime_Licensors)
                .ThenInclude(al => al.Licensor)
            .Include(a => a.Anime_Producers)
                .ThenInclude(ap => ap.Producer)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(condition);

        return anime;
    }
    public async Task<IEnumerable<Anime>> GetByConditionAsync(IEnumerable<Expression<Func<Anime, bool>>>? filters = null)
    {
        var query = _context.Anime
            .AsExpandable();

        if (filters is not null)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
        }

        var result = await query
            .OrderBy(a => a.Id)
            .AsNoTracking()
            .AsSplitQuery()
            .Select(a => new Anime
            {
                Id = a.Id,
                Name = a.Name,
                English_Name = a.English_Name,
                Other_Name = a.Other_Name,
                Synopsis = a.Synopsis,
                Image_URL = a.Image_URL,
                Type = a.Type,
                Episodes = a.Episodes,
                Duration = a.Duration,
                Source = a.Source,
                Release_Year = a.Release_Year,
                Started_Airing = a.Started_Airing,
                Finished_Airing = a.Finished_Airing,
                Rating = a.Rating,
                Studio = a.Studio,
                Score = a.Score,
                Status = a.Status,
                Anime_Genres = a.Anime_Genres.Select(ag => new Anime_Genre
                {
                    AnimeId = ag.AnimeId,
                    GenreId = ag.GenreId,
                    Genre = new Genre
                    {
                        Id = ag.Genre.Id,
                        Name = ag.Genre.Name
                    }
                }).ToList(),
                Anime_Licensors = a.Anime_Licensors.Select(al => new Anime_Licensor
                {
                    AnimeId = al.AnimeId,
                    LicensorId = al.LicensorId,
                    Licensor = new Licensor
                    {
                        Id = al.Licensor.Id,
                        Name = al.Licensor.Name
                    }
                }).ToList(),
                Anime_Producers = a.Anime_Producers.Select(ap => new Anime_Producer
                {
                    AnimeId = ap.AnimeId,
                    ProducerId = ap.ProducerId,
                    Producer = new Producer
                    {
                        Id = ap.Producer.Id,
                        Name = ap.Producer.Name
                    }
                }).ToList()
            })
            .Take(100)
            .ToListAsync();

        return result;
    }

    private void UpdateAnime(Anime original, Anime updated)
    {
        original.Name = updated.Name;
        original.English_Name = updated.English_Name;
        original.Other_Name = updated.Other_Name;
        original.Synopsis = updated.Synopsis;
        original.Image_URL = updated.Image_URL;
        original.Type = updated.Type;
        original.Episodes = updated.Episodes;
        original.Duration = updated.Duration;
        original.Source = updated.Source;
        original.Release_Year = updated.Release_Year;
        original.Started_Airing = updated.Started_Airing;
        original.Finished_Airing = updated.Finished_Airing;
        original.Rating = updated.Rating;
        original.Studio = updated.Studio;
        original.Score = updated.Score;
        original.Status = updated.Status;
        original.Anime_Genres
            .Update(updated.Anime_Genres, original.Id);
        original.Anime_Producers
            .Update(updated.Anime_Producers, original.Id);
        original.Anime_Licensors
            .Update(updated.Anime_Licensors, original.Id);
    }
}