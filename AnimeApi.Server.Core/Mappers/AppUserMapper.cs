using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using static AnimeApi.Server.Core.Constants;

namespace AnimeApi.Server.Core.Mappers;

public class AppUserMapper : Mapper<AppUser, AppUserDto>
{
    public override AppUserDto MapToDto(AppUser appUser)
    {
        return new AppUserDto
        {
            Id = appUser.Id,
            Email = appUser.Email,
            Username = appUser.Username,
            CreatedAt = appUser.Created_At,
            ProfilePictureUrl = appUser.Picture_Url,
            Admin = appUser.Role?.Access.EqualsIgnoreCase(UserAccess.Admin) ?? false
        };
    }

    public override AppUser MapToEntity(AppUserDto appUserDto)
    {
        return new AppUser
        {
            Id = appUserDto.Id,
            Email = appUserDto.Email,
            Username = appUserDto.Username,
            Created_At = appUserDto.CreatedAt,
            Picture_Url = appUserDto.ProfilePictureUrl,
            Role_Id = appUserDto.Admin 
                ? (int) UserAccess.Roles.Admin
                : (int) UserAccess.Roles.User,
        };
    }
}