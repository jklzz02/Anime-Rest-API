using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class SourceRepository : ISourceRepository
{
    private readonly AnimeDbContext _context;
    public Dictionary<string, string> ErrorMessages { get; } = new();

    public SourceRepository(AnimeDbContext context)
    {
        _context = context;
    } 
    
    public async Task<Source?> GetByIdAsync(int id)
    {
        return await _context.Sources
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Source>> GetByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        return await _context.Sources
            .AsNoTracking()
            .Where(s => EF.Functions.Like(s.Name, $"%{name}%"))
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<Source>> GetAllAsync()
    {
        return await _context.Sources
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    public async Task<Source?> AddAsync(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var type = await GetByIdAsync(entity.Id);
        if (type != null)
        {
            ErrorMessages.Add("id", $"Cannot add another source with id '{entity.Id}'");
            return null;
        }

        if (_context.Sources.Any(s => s.Name == entity.Name))
        {
            ErrorMessages.Add("name", $"Cannot add another source with name '{entity.Name}'");
            return null;
        }
        
        var createdEntry = _context.Sources.Add(entity);
        var result = await _context.SaveChangesAsync() > 0;
        return result ? createdEntry.Entity : null;
    }

    public async Task<Source?> UpdateAsync(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var source = await GetByIdAsync(entity.Id);
        if (source is null)
        {
            ErrorMessages.Add("id", $"There is no anime source with id '{entity.Id}'");
            return null;
        }
        if (_context.Sources.Any(s => s.Name == entity.Name && s.Id != entity.Id))
        {
            return null;
        }

        source.Name = entity.Name;
        var result = await _context.SaveChangesAsync() > 0;
        return result ? await GetByIdAsync(source.Id) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var source = await GetByIdAsync(id);
        if (source == null) return false;
        
        _context.Sources.Remove(source);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<int>> GetExistingIdsAsync()
    {
        return await _context.Sources
            .AsNoTracking()
            .Select(s => s.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetExistingNamesAsync()
    {
        return await _context.Sources
            .Select(s => s.Name!)
            .ToListAsync();
    }
}