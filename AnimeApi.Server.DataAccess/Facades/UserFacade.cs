using AnimeApi.Server.Core.Abstractions.DataAccess;
using AnimeApi.Server.Core.Abstractions.DataAccess.Facades;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.DataAccess.Facades;

public class UserFacade(
    IRoles roles,
    IRepository<Ban, BanDto> banRepo,
    IRepository<AppUser, AppUserDto> userRepo,
    IRepository<Favourite, FavouriteDto>  favRepo)
    : IUserFacade
{
    public IRoles Roles => roles;
    
    public IRepository<Ban, BanDto> Bans =>  banRepo;

    public IRepository<AppUser, AppUserDto> Users => userRepo;
    
    public IRepository<Favourite, FavouriteDto> Favourites =>  favRepo;
}