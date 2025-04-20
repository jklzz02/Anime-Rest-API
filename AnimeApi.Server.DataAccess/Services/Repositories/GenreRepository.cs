using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Model;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly AnimeDbContext _context;

    public GenreRepository(AnimeDbContext context)
    {
        _context = context;
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await _context.Genres
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<IEnumerable<Genre>> GetByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        return await _context.Genres
            .Where(g => EF.Functions.Like(g.Name, $"%{name}%"))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> AddAsync(Genre entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var genre = await GetByIdAsync(entity.Id);
        if (genre is not null) return false;
        
        await _context.Genres.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Genre entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var genre = await GetByIdAsync(entity.Id);
        if (genre is null) return false;
        _context.Genres.Update(entity);
        
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var genre = await GetByIdAsync(id);
        if (genre == null) return false;
        _context.Genres.Remove(genre);
        
        return await _context.SaveChangesAsync() > 0;
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