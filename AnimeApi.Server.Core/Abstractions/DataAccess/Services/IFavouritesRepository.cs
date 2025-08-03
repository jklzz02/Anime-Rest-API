using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Services;

public interface IFavouritesRepository
{
    Task<UserFavourites?> GetFavouriteAsync(int userId, int animeId);
    public Task<IEnumerable<int>> GetFavouritesAsync(int userId);
    public Task<bool> AddFavouriteAsync(int userId, int animeId);
    public Task<bool> RemoveFavouriteAsync(int userId, int animeId);
    public Task<int> GetFavouritesCountAsync(int animeId);
}