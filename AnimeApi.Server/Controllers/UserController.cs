using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Dto;
using AnimeApi.Server.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService, IFavouritesHelper favouritesHelper)
    : ControllerBase
{
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await userService.GetByEmailAsync(email?.Value ?? string.Empty);
        
        if (user is null)
        {
            return Unauthorized();
        }
        
        return Ok(user);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> GetAllAsync([FromQuery] Pagination pagination)
    {
        var users = await
            userService
                .GetPublicUsersAsync(pagination.Page, pagination.Size);
        
        return Ok(users);
    }

    [HttpGet("{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var user = await
            userService.GetPublicUserAsync(id);
        
        if (user is null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }

    [Authorize]
    [HttpGet("favourite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserFavourites()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null)
        {
            return Unauthorized();
        }

        var favourites = await favouritesHelper
            .GetFavouritesAsync(user.Id);
        
        return Ok(favourites);
    }

    [Authorize]
    [HttpPost("favourite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddFavouriteAsync(
        [FromBody, Range(1, int.MaxValue)] int animeId)
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null)
        {
            return Unauthorized();
        }

        var favourite = new FavouriteDto
        {
            UserId = user.Id,
            AnimeId = animeId
        };

        var favourites = await favouritesHelper.GetFavouritesAsync(user.Id);

        if (favourites.Any(f => f.UserId == user.Id && f.AnimeId == animeId))
        {
            return BadRequest();
        }
        
        var result = await favouritesHelper.AddFavouriteAsync(favourite);
        
        if (result.IsFailure)
        {
            return BadRequest(result.Errors.ToKeyValuePairs());
        }
        
        return Ok(favourite);
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DestroyCurrentUserAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null)
        {
            return NotFound();
        }
        
        var result = await userService.DestroyUserAsync(email!.Value);
        
        if (!result)
        {
            Unauthorized();
        }

        return NoContent();
    }

    [HttpDelete("destroy/{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    public async Task<IActionResult> DestroyUserAsync([FromRoute, Range(1, int.MaxValue)] int id)
    {
        var result = await userService.DestroyUserAsync(id);
        if (!result)
        {
            NotFound();
        }
        
        return NoContent();
    }
    
    [Authorize]
    [HttpDelete("favourite/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DestroyFavouriteAsync(
        [FromRoute, Range(1, int.MaxValue)] int id)
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await userService.GetByEmailAsync(email?.Value ?? string.Empty);
        
        if (user is null)
        {
            return Unauthorized();
        }

        var favourite = new FavouriteDto
        {
            UserId = user.Id,
            AnimeId = id
        };
        
        var result = await favouritesHelper.RemoveFavouriteAsync(favourite);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}