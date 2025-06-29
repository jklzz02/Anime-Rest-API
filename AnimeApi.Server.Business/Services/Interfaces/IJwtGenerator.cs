

using AnimeApi.Server.Business.Objects.Dto;

namespace AnimeApi.Server.Business.Services.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(AppUserDto user);
}