using AnimeApi.Server.Business.Dto;
using AnimeApi.Server.DataAccess.Models;

namespace AnimeApi.Server.Business.Extensions.Mappers;

public static class AppUserMapper
{
    public static AppUserDto ToDto(this AppUser appUser)
    {
        return new AppUserDto
        {
            Id = appUser.Id,
            Email = appUser.Email,
            Username = appUser.Username,
            CreatedAt = appUser.Created_At,
            ProfilePictureUrl = appUser.Picture_Url
        };
    }

    public static AppUser ToModel(this AppUserDto appUserDto)
    {
        return new AppUser
        {
            Id = appUserDto.Id,
            Email = appUserDto.Email,
            Username = appUserDto.Username,
            Created_At = appUserDto.CreatedAt,
            Picture_Url = appUserDto.ProfilePictureUrl
        };
    }
}