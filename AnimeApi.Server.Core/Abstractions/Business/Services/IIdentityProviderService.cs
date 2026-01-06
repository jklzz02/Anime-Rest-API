using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Auth;

namespace AnimeApi.Server.Core.Abstractions.Business.Services;

public interface IIdentityProviderService
{
   Task<Result<AuthPayload>> ProcessIdentityProviderAsync(AuthRequest request);
}