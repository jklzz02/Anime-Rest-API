using AnimeApi.Server.Core.Abstractions.DataAccess.Services;
using AnimeApi.Server.Core.Objects;
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

    public async Task<Result<Favourite>> AddFavouriteAsync(int userId, int animeId)
    {

        var entity = await GetFavouriteAsync(userId, animeId);
        
        if (entity != null)
        {
            return Result<Favourite>.ValidationFailure("Favourit" ,$"Favourite with anime id '{animeId}' and user id '{userId}' already exists.");
        }

        var newEntity = new Favourite
        {
            User_Id = userId,
            Anime_Id = animeId
        };
        
        var createdEntry = _context.User_Favourites.Add(newEntity);
        var result =  await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            return Result<Favourite>.InternalFailure("Create", "Failed to add favourite.");
        }

        return Result<Favourite>.Success(createdEntry.Entity);
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