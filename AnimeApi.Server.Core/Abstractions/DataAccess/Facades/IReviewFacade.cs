using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Facades;

public interface IReviewFacade
{
    IRepository<Review, ReviewDto>  Reviews { get; }
    
    IRepository<AppUser, AppUserDto> AppUsers { get; }
    
    IRepository<Anime, AnimeDto> Anime { get; }
}