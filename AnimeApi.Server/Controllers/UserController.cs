using System.Security.Claims;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
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
}