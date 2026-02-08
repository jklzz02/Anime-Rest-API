using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;

namespace AnimeApi.Server.Core.Abstractions.DataAccess.Facades;

public interface IUserFacade
{
    IRoles Roles { get; }
    
    IRepository<Ban,  BanDto> Bans { get; }
    
    IRepository<AppUser, AppUserDto>  Users { get; }
    
    IRepository<Favourite, FavouriteDto> Favourites { get; }
}