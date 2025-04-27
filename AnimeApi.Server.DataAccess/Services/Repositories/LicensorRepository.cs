using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Model;
using AnimeApi.Server.DataAccess.Services.Interfaces;
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

    public async Task<IEnumerable<Licensor>> GetAllAsync()
    {
        return await _context.Licensors.ToListAsync();
    }

    public async Task<bool> AddAsync(Licensor entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
    
        var licensor = await GetByIdAsync(entity.Id);
        if (licensor is not null) return false;
        
        _context.Licensors.Add(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Licensor entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        
        var licensor = await GetByIdAsync(entity.Id);
        if(licensor is null) return false;

        _context.Licensors.Update(entity);
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