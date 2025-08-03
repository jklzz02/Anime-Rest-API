using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects.Models;
using AnimeApi.Server.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace AnimeApi.Server.DataAccess.Repositories;

public class FavouritesRepository : IFavouritesRepository
{
    private readonly AnimeDbContext _context;
    
    public FavouritesRepository(AnimeDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<int>> GetFavouritesAsync(int userId)
    {
        return await _context.User_Favourites
            .AsNoTracking()
            .Where(uf => uf.User_Id == userId)
            .Select(uf => uf.Anime_Id)
            .ToListAsync();
    }

    public async Task<bool> AddFavouriteAsync(int userId, int animeId)
    {

        var entity = await GetFavouriteAsync(userId, animeId);
        
        if (entity != null)
        {
            return false;
        }

        var newEntity = new Favourite
        {
            User_Id = userId,
            Anime_Id = animeId
        };
        
        _context.User_Favourites.Add(newEntity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveFavouriteAsync(int userId, int animeId)
    {

        var entity = await GetFavouriteAsync(userId, animeId);
        
        if (entity is null)
        {
            return false;
        }
        
        _context.User_Favourites.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
    

    public async Task<int> GetFavouritesCountAsync(int animeId)
    {
        var query = _context.User_Favourites
            .AsNoTracking()
            .Where(uf => uf.Anime_Id == animeId);
        
        return await query.CountAsync();
    }

    public async Task<Favourite?> GetFavouriteAsync(int userId, int animeId)
    {
        return await _context.User_Favourites
            .FirstOrDefaultAsync(uf => uf.User_Id == userId && uf.Anime_Id == animeId);
    }
}