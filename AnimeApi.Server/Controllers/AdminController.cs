using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Authorize(Policy = Constants.UserAccess.Admin)]
[Route("[controller]")]
public class AdminController : Controller
{
    [HttpDelete("cache/clear")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public Task<IActionResult> ClearCache([FromServices] ICachingService cache)
    {
        try
        {
            cache.Clear();
            return Task.FromResult<IActionResult>(NoContent());
        }
        catch (Exception ex)
        {
            return Task.FromResult<IActionResult>(
                StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new
                    {
                        error = Constants.HttpRemark.Unavailable,
                        details = ex.Message 
                    }));
        }
    }

    [HttpGet("linked-users-details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserDetails(
        [FromServices] IUserService userService,
        [FromServices] IReviewHelper reviewHelper,
        [FromServices] IBanService banService,
        UserDetailsRequest request)
    {
        var users = (await
            userService.GetUsersLinkedToEmail(request.Email)).ToList();

        if (!users.Any())
        {
            return BadRequest($"There's no user with email '{request.Email}'");
        }
        
        var reviews = (await 
            reviewHelper.GetByUserEmailAsync(request.Email))
                .ToLookup(r => r.UserId);
        
        var ban = (await
            banService.GetBanHistoryAsync(request.Email))
                .ToLookup(b => b.UserId);

        var userDetails = users.Select(u => new
        {
            user = u,
            reviews = reviews[u.Id],
            ban = ban[u.Id]
        });
        
        return Ok(userDetails);
    }
}