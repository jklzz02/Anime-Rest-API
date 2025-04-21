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
        
        return await GetByConditionAsync(a => EF.Functions.Like(a.Name, $"%{name}%"));
    }

    public async Task<IEnumerable<Anime>> GetByEnglishNameAsync(string englishName)
    {
        ArgumentNullException.ThrowIfNull(englishName, nameof(englishName));
        ArgumentException.ThrowIfNullOrEmpty(englishName, nameof(englishName));
        
        return await GetByConditionAsync(a => EF.Functions.Like(a.English_Name, $"%{englishName}%"));
    }

    public async Task<IEnumerable<Anime>> GetBySourceAsync(string source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentException.ThrowIfNullOrEmpty(source, nameof(source));
        
        return await GetByConditionAsync(a => EF.Functions.Like(a.Source, $"%{source}%"));
    }

    public async Task<IEnumerable<Anime>> GetByTypeAsync(string type)
    {
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        ArgumentException.ThrowIfNullOrEmpty(type, nameof(type));
        
        return await GetByConditionAsync(a => EF.Functions.Like(a.Type, $"%{type}%"));
    }

    public async Task<IEnumerable<Anime>> GetByScoreAsync(int score)
    {
        return await GetByConditionAsync(a => a.Score == score);
    }

    public async Task<IEnumerable<Anime>> GetByLicensorAsync(int licensorId)
    {
        return await GetByConditionAsync(a => a.Anime_Licensors.Any(al => al.LicensorId == licensorId));
    }

    public async Task<IEnumerable<Anime>> GetByProducerAsync(int producerId)
    {
        return await GetByConditionAsync(a => a.Anime_Producers.Any(ap => ap.ProducerId == producerId));
    }

    public async Task<IEnumerable<Anime>> GetByGenreAsync(int genreId)
    {
        return await GetByConditionAsync(a => a.Anime_Genres.Any(ag => ag.GenreId == genreId));
    }

    public async Task<IEnumerable<Anime>> GetByReleaseYearAsync(int year)
    {
        return await GetByConditionAsync(a => a.Release_Year == year);
    }
    
    public async Task<Anime?> GetFirstByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        return await _context.Anime
            .AsNoTracking()
            .FirstOrDefaultAsync(condition);
    }

    public async Task<IEnumerable<Anime>> GetByConditionAsync(Expression<Func<Anime, bool>> condition)
    {
        return await _context.Anime
            .AsExpandable()
            .Where(condition)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}