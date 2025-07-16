using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using static AnimeApi.Server.Business.Constant;

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
            ProfilePictureUrl = appUser.Picture_Url,
            Admin = appUser.Role.Access.EqualsIgnoreCase(UserAccess.Admin)
        };
    }

    public static AppUser ToModel(this AppUserDto appUserDto, int roleId)
    {
        return new AppUser
        {
            Id = appUserDto.Id,
            Email = appUserDto.Email,
            Username = appUserDto.Username,
            Created_At = appUserDto.CreatedAt,
            Picture_Url = appUserDto.ProfilePictureUrl,
            Role_Id = roleId
        };
    }
}