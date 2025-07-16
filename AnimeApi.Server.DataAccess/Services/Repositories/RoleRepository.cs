using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AnimeDbContext _context;
    
    public RoleRepository(AnimeDbContext context)
    {
        _context = context;
    }
    
    public async Task<Role?> GetByAccessAsync(string access)
    {
        ArgumentNullException.ThrowIfNull(access, nameof(access));
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Access == access);
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}