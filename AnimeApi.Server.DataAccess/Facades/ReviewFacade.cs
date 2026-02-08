using AnimeApi.Server.Core.Abstractions.DataAccess;
using AnimeApi.Server.Core.Abstractions.DataAccess.Facades;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.DataAccess.Facades;

public class ReviewFacade(
    IRepository<Review, ReviewDto> reviewRepo,
    IRepository<AppUser, AppUserDto> userRepo,
    IRepository<Anime, AnimeDto>  animeRepo)
    : IReviewFacade
{
    public IRepository<Review, ReviewDto> Reviews => reviewRepo;
    
    public IRepository<AppUser, AppUserDto> AppUsers => userRepo;
    
    public IRepository<Anime, AnimeDto> Anime =>  animeRepo;
}