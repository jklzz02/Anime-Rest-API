using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AnimeDbContext _context;

    public RefreshTokenRepository(AnimeDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Hashed_Token.Equals(token));
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> GetByIdAsync(int id)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> GetByUserIdAsync(int userId)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.User_Id == userId);
    }
    
    /// <inheritdoc/>

    public async Task<RefreshToken?> AddAsync(RefreshToken entity)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == entity.User_Id);
        
        if (user is null) 
            return null;
        
        await _context.RefreshTokens.AddAsync(entity);
        var result = await _context.SaveChangesAsync() > 0;
        return result ? entity : null;
    }

    /// <inheritdoc/>
    public async Task<bool> RevokeByUserIdAsync(int userId)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.User_Id == userId);
        
        if (refreshToken is null) 
            return false;
        
        refreshToken.Revoked_At = DateTime.UtcNow;
        return await _context.SaveChangesAsync() > 0;
    }
    
    /// <inheritdoc/>
    public async Task<bool> RevokeAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Hashed_Token == token);
        
        if (refreshToken is null) 
            return false;
        
        refreshToken.Revoked_At = DateTime.UtcNow;
        return await _context.SaveChangesAsync() > 0;
    }
    
    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        var token = await GetByIdAsync(id);
        if (token is null) return false;
        
        _context.RefreshTokens.Remove(token);
        return await _context.SaveChangesAsync() > 0;
    }
    
    /// <inheritdoc/>
    public async Task<bool> DeleteByUserIdAsync(int userId)
    {
        var token = await GetByUserIdAsync(userId);
        if (token is null) 
            return false;
        
        _context.RefreshTokens.Remove(token);
        return await _context.SaveChangesAsync() > 0;
    }
}