using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Type = AnimeApi.Server.Core.Objects.Models.Type;

namespace AnimeApi.Server.DataAccess.Repositories;

public class TypeRepository : ITypeRepository
{
    private readonly AnimeDbContext _context;

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

    public async Task<Result<Type>> AddAsync(Type entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        List<Error> errors = [];

        var type = await GetByIdAsync(entity.Id);
        if (type != null)
        {
            errors.Add(Error.Validation("id", $"Cannot add another anime type with id '{entity.Id}'"));
        }

        if (_context.Types.Any(t => t.Name == entity.Name && t.Id != entity.Id))
        {
            errors.Add(Error.Validation("name", $"Cannot add another anime type with name '{entity.Name}'"));
        }

        if (errors.Any())
        {
            return Result<Type>.Failure(errors);
        }
        
        var createdEntry = await _context.Types.AddAsync(entity);
        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result)
            return Result<Type>.InternalFailure("create", "something went wrong during entity creation.");
            
        return Result<Type>.Success(createdEntry.Entity);
    }

    public async Task<Result<Type>> UpdateAsync(Type entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        List<Error> errors = [];
        
        var type = await GetByIdAsync(entity.Id);
        if (type is null)
        {
            errors.Add(Error.Validation("id", $"There is no anime type with id '{entity.Id}'"));
        }
        
        if (_context.Types.Any(t => t.Name == entity.Name && t.Id != entity.Id))
        {
            errors.Add(Error.Validation("name", $"There is already an anime type with name '{entity.Name}'"));
        }

        if (errors.Any())
        {
            return Result<Type>.Failure(errors);
        }

        type.Name = entity.Name;
        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result)
            return Result<Type>.InternalFailure("update", "something went wrong during entity update.");
            
        return Result<Type>.Success(type);
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