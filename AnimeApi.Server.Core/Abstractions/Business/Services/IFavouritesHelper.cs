using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IFavouritesHelper
{
    Dictionary<string, string> ErrorMessages { get; }
    Task<FavouriteDto?> GetFavouriteAsync(int userId, int animeId);
    Task<IEnumerable<FavouriteDto>> GetFavouritesAsync(int userId);
    Task<bool> AddFavouriteAsync(FavouriteDto favourite);
    Task<bool> RemoveFavouriteAsync(FavouriteDto favourite);
    Task<int> GetFavouritesCountAsync(int animeId);
}