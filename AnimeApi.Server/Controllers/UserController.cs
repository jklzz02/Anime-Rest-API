using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AnimeApi.Server.Business.Services.Interfaces;
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
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);
        
        if (user is null) return Unauthorized();
        
        return Ok(user);
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DestroyCurrentUserAsync()
    {
        var email = User.FindFirst(JwtRegisteredClaimNames.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null) return NotFound();
        
        var result = await _userService.DestroyUserAsync(email!.Value);
        
        if(!result) return Unauthorized();

        return NoContent();
    }
}