using System.Security.Claims;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using Microsoft.AspNetCore.Authorization;

namespace AnimeApi.Server.Handlers;

public class BanAuthorizationHandler(IBanService banService) : IAuthorizationHandler
{
    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        var useridClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        
        if (useridClaim is null)
        {
            return;
        }

        if (!int.TryParse(useridClaim.Value, out int userId))
        {
            context.Fail();
            return;
        }
        
        var ban = await
            banService.GetActiveBanAsync(userId);

        if (ban is not null)
        {
            context.Fail(new AuthorizationFailureReason(
                this,
                ban.Reason ?? "Banned"));
        }
    }
}