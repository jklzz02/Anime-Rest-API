using System.Security.Claims;
using AnimeApi.Server.Business;
using AnimeApi.Server.Core;
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
    [ProducesResponseType(Constants.StatusCode.Ok)]
    [ProducesResponseType(Constants.StatusCode.Unauthorized)]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);
        
        if (user is null) return Unauthorized();
        
        return Ok(user);
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(Constants.StatusCode.NoContent)]
    [ProducesResponseType(Constants.StatusCode.Unauthorized)]
    [ProducesResponseType(Constants.StatusCode.NotFound)]
    public async Task<IActionResult> DestroyCurrentUserAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null) return NotFound();
        
        var result = await _userService.DestroyUserAsync(email!.Value);
        
        if(!result) return Unauthorized();

        return NoContent();
    }
}