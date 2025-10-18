using AnimeApi.Server.Core.Abstractions.Business.Mappers;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.Core.Objects.Models;
using static AnimeApi.Server.Core.Constants;

namespace AnimeApi.Server.Business.Extensions.Mappers;

public class AppUserMapper : IMapper<AppUser, AppUserDto>
{
    public AppUserDto MapToDto(AppUser appUser)
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

    public IEnumerable<AppUserDto> MapToDto(IEnumerable<AppUser> entities)
    {
        throw new NotImplementedException();
    }

    public AppUser MapToEntity(AppUserDto appUserDto, int roleId)
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

    public AppUser MapToEntity(AppUserDto dto)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<AppUser> MapToEntity(IEnumerable<AppUserDto> dtos)
    {
        throw new NotImplementedException();
    }
}