using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.DataAccess;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess;

public class Roles : IRoles
{
    private readonly AnimeDbContext _context;
    
    public Roles(AnimeDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Role> AdminAsync()
    {
        return await
            _context.Roles
                .FirstAsync(r => r.Access == Constants.UserAccess.Admin);
    }
    
    /// <inheritdoc/>
    public async Task<Role> UserAsync()
    {
        return await
            _context.Roles
                .FirstAsync(r => r.Access == Constants.UserAccess.User);
    }
}