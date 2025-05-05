using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class LicensorRepository : ILicensorRepository
{
    private readonly AnimeDbContext _context;
    public Dictionary<string, string> ErrorMessages { get; } = new();
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

    public async Task<IEnumerable<Licensor>> GetAllAsync()
    {
        return await _context.Licensors.ToListAsync();
    }

    public async Task<bool> AddAsync(Licensor entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
    
        var licensor = await GetByIdAsync(entity.Id);
        if (licensor is not null)
        {
            ErrorMessages.Add("id", "There is already a licensor with this id");
            return false;
        }

        if (_context.Licensors.Any(l => l.Name == entity.Name))
        {
            ErrorMessages.Add("name", "There is already a licensor with this name");
            return false;
        }
        
        _context.Licensors.Add(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Licensor entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var licensor = await GetByIdAsync(entity.Id);
        if(licensor is null) return false;
        if (_context.Licensors.Any(l => l.Name == entity.Name && l.Id != entity.Id))
        {
            return false;
        }

        licensor.Name = entity.Name;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var licensor = await GetByIdAsync(id);
        if (licensor is null) return false;
        
        _context.Licensors.Remove(licensor);
        return await _context.SaveChangesAsync() > 0;
    }
}