using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories;

public class ProducerRepository : IProducerRepository
{
    private readonly AnimeDbContext _context;
    
    public ProducerRepository(AnimeDbContext context)
    {
        _context = context;
    }
    
    public async Task<Producer?> GetByIdAsync(int id)
    {
        return await _context.Producers.FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<IEnumerable<Producer>> GetByNameAsync(string name)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        
        return await _context.Producers
            .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<int>> GetExistingIdsAsync()
    {
        return await _context.Producers
            .AsNoTracking()
            .Select(g => g.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetExistingNamesAsync()
    {
        return await _context.Producers
            .AsNoTracking()
            .Select(g => g.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Producer>> GetAllAsync()
    {
        return await _context.Producers
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    public async Task<Result<Producer>> AddAsync(Producer entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        List<Error> errors = [];
        
        var producer = await GetByIdAsync(entity.Id);
        if (producer is not null)
        {
            errors.Add(Error.Validation("id", $"Cannot add another producer with id '{entity.Id}'"));
        }

        if (_context.Producers.Any(p => p.Name == entity.Name))
        {
            errors.Add(Error.Validation("name", $"Cannot add another producer with name {entity.Name}"));
        }

        if (errors.Any())
        {
            return Result<Producer>.Failure(errors);
        }
        
        var createdEntry = await _context.Producers.AddAsync(entity);
        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
            return Result<Producer>.InternalFailure("create", "something went wrong during entity creation.");
        
        return Result<Producer>.Success(createdEntry.Entity);
    }

    public async Task<Result<Producer>> UpdateAsync(Producer entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        List<Error> errors = [];
        
        var producer = await GetByIdAsync(entity.Id);
        if (producer is null)
        {
            errors.Add(Error.Validation("id", $"There is no producer with id '{entity.Id}'"));
        }

        if (_context.Producers.Any(p => p.Name == entity.Name && p.Id != entity.Id))
        {
            errors.Add(Error.Validation("name", $"There is already a producer with name {entity.Name}"));
        }

        if (errors.Any())
        {
            return Result<Producer>.Failure(errors);
        }
        
        producer.Name = entity.Name;
        var result = await _context.SaveChangesAsync() > 0;
        
        if (!result)
            return Result<Producer>.InternalFailure("update", "something went wrong during entity update.");
        
        return Result<Producer>.Success(producer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var producer = await GetByIdAsync(id);
        if (producer is null) return false;
        
        _context.Producers.Remove(producer);
        return await _context.SaveChangesAsync() > 0;
    }
}