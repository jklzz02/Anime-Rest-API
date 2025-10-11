using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IFavouritesHelper
{
    Task<FavouriteDto?> GetFavouriteAsync(int userId, int animeId);
    Task<IEnumerable<FavouriteDto>> GetFavouritesAsync(int userId);
    Task<Result<FavouriteDto>> AddFavouriteAsync(FavouriteDto favourite);
    Task<bool> RemoveFavouriteAsync(FavouriteDto favourite);
    Task<int> GetFavouritesCountAsync(int animeId);
}