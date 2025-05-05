using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly AnimeDbContext _context;
    public Dictionary<string, string> ErrorMessages { get; private set; } = new();
    
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
        return await _context.Genres
            .OrderBy(g => g.Id)
            .AsNoTracking()
            .ToListAsync();
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
        if (genre is not null)
        {
            ErrorMessages.Add("id", "There is already a genre with this id");
            return false;
        }
        if (_context.Genres.Any(g => g.Name == entity.Name))
        {
            ErrorMessages.Add("name", "There is already a genre with this name");
            return false;
        }
        
        await _context.Genres.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Genre entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var genre = await GetByIdAsync(entity.Id);
        if (genre is null)
        {
            ErrorMessages.Add("id", "There is no genre with this id");
            return false;
        }

        if (_context.Genres.Any(g => g.Name == entity.Name && g.Id != entity.Id))
        {
            ErrorMessages.Add("name", "There is already a genre with this name");
            return false;
        }
        
        genre.Name = entity.Name;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var genre = await GetByIdAsync(id);
        if (genre == null) return false;
        _context.Genres.Remove(genre);
        
        return await _context.SaveChangesAsync() > 0;
    }
}