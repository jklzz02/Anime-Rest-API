using System.Linq.Expressions;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Model;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class AnimeRepository : IAnimeRepository
{
    private readonly AnimeDbContext _context;

    public AnimeRepository(AnimeDbContext context)
    {
        _context = context;
    }

    public async Task<Anime?> GetByIdAsync(int id)
    {
        return await GetFirstByConditionAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Anime>> GetAllAsync()
    {
        return await _context.Anime.ToListAsync();
    }

    public async Task<bool> AddAsync(Anime entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var anime = await GetByIdAsync(entity.Id);
        if (anime is not null) return false;
        
        _context.Anime.Add(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Anime entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var anime = await GetByIdAsync(entity.Id);
        if (anime is null) return false;
        
        _context.Anime.Update(entity);
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
            .Take(100)
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
            .ToListAsync();

        return result;
    }
}