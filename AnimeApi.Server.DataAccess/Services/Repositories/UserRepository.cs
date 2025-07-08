using AnimeApi.Server.DataAccess.Context;
using AnimeApi.Server.DataAccess.Models;
using AnimeApi.Server.DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Services.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AnimeDbContext _context;
    
    public UserRepository(AnimeDbContext context)
    {
        _context = context;
    }
    
    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return user;
    }

    public async Task<AppUser?> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        
        return user;
    }

    public async Task<bool?> CreateAsync(AppUser user)
    {
        _context.Users.Add(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DestroyAsync(string email)
    {
        var user = await GetByEmailAsync(email);
        if (user is null) return false;
        
        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }
}