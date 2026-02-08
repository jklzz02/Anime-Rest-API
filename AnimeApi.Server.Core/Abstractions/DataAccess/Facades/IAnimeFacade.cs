using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Facades;

public interface IAnimeFacade
{
    IRepository<Anime, AnimeDto> Anime { get; }
    
    IRepository<Favourite, FavouriteDto> Favourites { get; }
    
    IRepository<Review, ReviewDto> Reviews { get; }
}