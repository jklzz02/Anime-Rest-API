using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories;

public class SourceRepository : ISourceRepository
{
    private readonly AnimeDbContext _context;

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

    public async Task<Result<Source>> AddAsync(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        List<Error> errors = [];

        var type = await GetByIdAsync(entity.Id);
        if (type != null)
        {
            errors.Add(Error.Validation("id", $"Cannot add another source with id '{entity.Id}'"));
        }

        if (_context.Sources.Any(s => s.Name == entity.Name))
        {
            errors.Add(Error.Validation("name", $"Cannot add another source with name '{entity.Name}'"));
        }

        if (errors.Any())
        {
            return Result<Source>.Failure(errors);
        }
        
        var createdEntry = await _context.Sources.AddAsync(entity);
        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Source>.InternalFailure("create", "something went wrong during entity creation.");
        
        return Result<Source>.Success(createdEntry.Entity);
    }

    public async Task<Result<Source>> UpdateAsync(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        List<Error> errors = [];
        
        var source = await GetByIdAsync(entity.Id);
        if (source is null)
        {
            errors.Add(Error.Validation("id", $"There is no anime source with id '{entity.Id}'"));
        }
        
        if (_context.Sources.Any(s => s.Name == entity.Name && s.Id != entity.Id))
        {
            errors.Add(Error.Validation("name", $"There is already a source with name '{entity.Name}'"));
        }

        if (errors.Any())
        {
            return Result<Source>.Failure(errors);
        }

        source.Name = entity.Name;
        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result)
            return Result<Source>.InternalFailure("update", "something went wrong during entity update.");
        
        return Result<Source>.Success(source);
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