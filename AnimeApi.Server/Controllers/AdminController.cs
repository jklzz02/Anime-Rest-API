using System.ComponentModel.DataAnnotations;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Authorize(Policy = Constants.UserAccess.Admin)]
[Route("[controller]")]
public class AdminController(
    IBanService banService,
    IUserService userService,
    IReviewHelper reviewHelper) : Controller
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

    [HttpGet("user-details/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserDetails(
        [FromRoute, Range(1, int.MaxValue)] int userId)
    {
        var user = await
            userService.GetByIdAsync(userId);

        if (user == null)
        {
            return BadRequest($"User with id '{userId}' not found");
        }

        var reviews = await
            reviewHelper.GetByUserIdAsync(user.Id);
        
        var banHistory  = await banService.GetBanHistoryAsync(user.Email);

        return Ok(new
        {
            user = user,
            reviews = reviews,
            ban = banHistory
        });
    }

    [HttpGet("linked-users-details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetLinkedUsersDetails(UserDetailsRequest request)
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
    
    
    [HttpPost("ban")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> BanUserAsync(
        [FromBody] BanRequest request,
        [FromServices] IRefreshTokenService refreshTokenService)
    {
        var res = await
            banService.BanUserAsync(request.Email, request.Expiration, request.Reason);

        if (res.IsFailure)
        {
            return BadRequest(res.ValidationErrors.ToKeyValuePairs());
        }

        await refreshTokenService.RevokeByEmailAsync(request.Email);
        
        return NoContent();
    }

    [HttpPatch("unban")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UnbanUserAsync([FromQuery, MinLength(1), MaxLength(250)] string email)
    {
        var res = await banService.UnbanUserAsync(email);
        if (res.IsFailure)
        {
            return BadRequest(res.ValidationErrors.ToKeyValuePairs());
        }

        return Ok(res.Data);
    }
}