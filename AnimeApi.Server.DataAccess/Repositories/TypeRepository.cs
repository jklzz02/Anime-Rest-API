using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class TypeRepository : ITypeRepository
{
    private readonly AnimeDbContext _context;
    public Dictionary<string, string> ErrorMessages { get; } = new();

    public TypeRepository(AnimeDbContext context)
    {
        _context = context;
    }
    
    public async Task<Type?> GetByIdAsync(int id)
    {
        return await _context.Types.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Type>> GetByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        return await _context.Types
            .AsNoTracking()
            .Where(t => EF.Functions.Like(t.Name, $"%{name}%"))
            .ToListAsync();
    }

    public async Task<IEnumerable<Type>> GetAllAsync()
    {
        return await _context.Types
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .ToListAsync();
    }

    public async Task<Type?> AddAsync(Type entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        var type = await GetByIdAsync(entity.Id);

        if (type != null)
        {
            ErrorMessages.Add("id", $"Cannot add another anime type with id '{entity.Id}'");
            return null;
        }

        if (_context.Types.Any(t => t.Name == entity.Name && t.Id != entity.Id))
        {
            ErrorMessages.Add("name", $"Cannot add another anime type with name '{entity.Name}'");
            return null;
        }
        
        var createdEntry = _context.Types.Add(entity);
        var result = await _context.SaveChangesAsync() > 0;
        return result ? createdEntry.Entity : null;
    }

    public async Task<Type?> UpdateAsync(Type entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var type = await GetByIdAsync(entity.Id);
        if (type is null)
        {
            ErrorMessages.Add("id", $"There is no anime type with id '{entity.Id}'");
            return null;
        }
        if (_context.Sources.Any(t => t.Name == entity.Name && t.Id != entity.Id))
        {
            return null;
        }

        type.Name = entity.Name;
        var result = await _context.SaveChangesAsync() > 0;
        return result ? await GetByIdAsync(type.Id) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var type = await GetByIdAsync(id);
        if (type == null) return false;
        
        _context.Types.Remove(type);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<int>> GetExistingIdsAsync()
    {
        return await _context.Types
            .AsNoTracking()
            .Select(t => t.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetExistingNamesAsync()
    {
        return await _context.Types
            .AsNoTracking()
            .Select(t => t.Name!)
            .ToListAsync();
    }
}