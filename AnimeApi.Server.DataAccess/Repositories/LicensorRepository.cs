using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class LicensorRepository : ILicensorRepository
{
    private readonly AnimeDbContext _context;
    public LicensorRepository(AnimeDbContext context)
    {
        _context = context;
    }
    
    public async Task<Licensor?> GetByIdAsync(int id)
    {
        return await _context.Licensors.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Licensor>> GetByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        return await _context.Licensors
            .Where(l => EF.Functions.Like(l.Name, $"%{name}%"))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<int>> GetExistingIdsAsync()
    {
        return await _context.Licensors
            .AsNoTracking()
            .Select(l => l.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetExistingNamesAsync()
    {
        return await _context.Licensors
            .AsNoTracking()
            .Select(l => l.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Licensor>> GetAllAsync()
    {
        return await _context.Licensors
            .AsNoTracking()
            .OrderBy(l => l.Id)
            .ToListAsync();
    }

    public async Task<Result<Licensor>> AddAsync(Licensor entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        List<Error> errors = [];
        
        var licensor = await GetByIdAsync(entity.Id);
        if (licensor is not null)
        {
            errors.Add(Error.Validation("id", $"Cannot add another licensor with id '{entity.Id}'"));
        }

        if (_context.Licensors.Any(l => l.Name == entity.Name))
        {
            errors.Add(Error.Validation("name", $"Cannot add another licensor with name '{entity.Name}'"));
        }

        if (errors.Any())
        {
            return Result<Licensor>.Failure(errors);       
        }
        
        var createdEntry = _context.Licensors.Add(entity);
        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result)
            return Result<Licensor>.InternalFailure("create", "something went wrong during entity creation.");
        
        return Result<Licensor>.Success(createdEntry.Entity);
    }

    public async Task<Result<Licensor>> UpdateAsync(Licensor entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        List<Error> errors = [];
        
        var licensor = await GetByIdAsync(entity.Id);
        if (licensor is null)
        {
            errors.Add(Error.Validation("id", $"There is no licensor with id '{entity.Id}'"));
        }
        
        if (_context.Licensors.Any(l => l.Name == entity.Name && l.Id != entity.Id))
        {
            errors.Add(Error.Validation("name", $"There is already a licensor with name '{entity.Name}'"));
        }

        if (errors.Any())
        {
            return Result<Licensor>.Failure(errors);
        }

        licensor.Name = entity.Name;
        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result)
            return Result<Licensor>.InternalFailure("update", "something went wrong during entity update.");
        
        return Result<Licensor>.Success(licensor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var licensor = await GetByIdAsync(id);
        if (licensor is null) return false;
        
        _context.Licensors.Remove(licensor);
        return await _context.SaveChangesAsync() > 0;
    }
}