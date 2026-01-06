using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Auth;
using AnimeApi.Server.Core.Objects.Dto;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IIdentityProviderService
{
   Task<Result<AppUserDto>> ProcessIdentityProviderAsync(AuthRequest request);
}