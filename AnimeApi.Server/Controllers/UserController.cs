using System.Security.Claims;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IFavouritesHelper _favouritesHelper;
    
    public UserController(IUserService userService, IFavouritesHelper favouritesHelper)
    {
        _userService = userService;
        _favouritesHelper = favouritesHelper;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);
        
        if (user is null) 
        {
            return Unauthorized();
        }
        
        return Ok(user);
    }

    [HttpGet]
    [Authorize]
    [Route("favourite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserFavourites()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null)
        {
            return Unauthorized();
        }

        var favourites = await _favouritesHelper
            .GetFavouritesAsync(user.Id);
        
        return Ok(favourites);
    }

    [HttpPost]
    [Authorize]
    [Route("favourite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddFavouriteAsync([FromBody] int animeId)
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null)
        {
            return Unauthorized();
        }

        var favourite = new FavouriteDto
        {
            UserId = user.Id,
            AnimeId = animeId
        };
        
        var result = await _favouritesHelper.AddFavouriteAsync(favourite);
        
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
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null) 
        {
            return NotFound();
        }
        
        var result = await _userService.DestroyUserAsync(email!.Value);
        
        if (!result) 
        {
            Unauthorized();
        }

        return NoContent();
    }
    
    [HttpDelete]
    [Authorize]
    [Route("favourite/{id:int:min(1)}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DestroyFavouriteAsync([FromRoute] int id)
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);
        
        if (user is null)
        {
            return Unauthorized();
        }

        var favourite = new FavouriteDto
        {
            UserId = user.Id,
            AnimeId = id
        };
        
        var result = await _favouritesHelper.RemoveFavouriteAsync(favourite);
        
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}