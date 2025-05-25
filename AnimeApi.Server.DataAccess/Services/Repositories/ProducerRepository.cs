using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class ProducerRepository : IProducerRepository
{
    private readonly AnimeDbContext _context;
    public Dictionary<string, string> ErrorMessages { get; } = new();
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
            .ToListAsync();
    }

    public async Task<Producer?> AddAsync(Producer entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var producer = await GetByIdAsync(entity.Id);
        if (producer is not null)
        {
            ErrorMessages.Add("id", $"There is already a producer with id '{entity.Id}'");
            return null;
        }

        if (_context.Producers.Any(p => p.Name == entity.Name))
        {
            ErrorMessages.Add("name", $"There is already a producer with name {entity.Name}");
            return null;
        }
        
        var createdEntry = _context.Producers.Add(entity);
        var result = await _context.SaveChangesAsync() > 0;
        return result ? createdEntry.Entity : null;
    }

    public async Task<Producer?> UpdateAsync(Producer entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var producer = await GetByIdAsync(entity.Id);
        if (producer is null)
        {
            ErrorMessages.Add("id", $"There is no producer with id '{entity.Id}'");
            return null;
        }

        if (_context.Producers.Any(p => p.Name == entity.Name && p.Id != entity.Id))
        {
            ErrorMessages.Add("name", $"There is already a producer with name {entity.Name}");
            return null;
        }
        
        producer.Name = entity.Name;
        var result = await _context.SaveChangesAsync() > 0;
        return result ? await GetByIdAsync(entity.Id) : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var producer = await GetByIdAsync(id);
        if (producer is null) return false;
        
        _context.Producers.Remove(producer);
        return await _context.SaveChangesAsync() > 0;
    }
}