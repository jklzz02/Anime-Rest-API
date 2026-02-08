using AnimeApi.Server.Core.Abstractions.DataAccess;
using AnimeApi.Server.Core.Abstractions.DataAccess.Facades;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.DataAccess.Facades;

public class AnimeFacade(
    IRepository<Anime, AnimeDto>  animeRepo,
    IRepository<Favourite, FavouriteDto> favRepo,
    IRepository<Review, ReviewDto> reviewRepo)
    : IAnimeFacade
{
    public IRepository<Anime, AnimeDto> Anime =>  animeRepo;
    
    public IRepository<Favourite, FavouriteDto> Favourites => favRepo;
    
    public IRepository<Review, ReviewDto> Reviews =>  reviewRepo;
}