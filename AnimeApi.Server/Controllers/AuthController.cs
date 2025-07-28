using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("[Controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthController(
        IUserService userService,
        IJwtGenerator jwtGenerator,
        IRefreshTokenService refreshTokenService)
    {
        _userService = userService;
        _jwtGenerator = jwtGenerator;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("google")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        GoogleJsonWebSignature.Payload payload;

        if (string.IsNullOrEmpty(request?.IdToken))
        {
            return BadRequest("Google ID token is required.");
        }
        
        try
        {
            payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
        }
        catch (InvalidJwtException)
        {
            return Unauthorized("Invalid Google ID token.");
        }

        var userDto = await _userService.GetOrCreateUserAsync(payload);
        var accessToken = _jwtGenerator.GenerateToken(userDto);
        var refreshToken = await _refreshTokenService.CreateAsync(userDto.Id);

        return Ok(new
        {
            access_token = accessToken,
            refresh_token = refreshToken,
            user = userDto
        });
    }
    
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var validation = await _refreshTokenService.ValidateAsync(request.RefreshToken);
        if (!validation.Success)
            return Unauthorized("Invalid or expired refresh token.");

        var user = await _userService.GetByIdAsync(validation.UserId);
        if (user is null)
            return Unauthorized("User not found.");

        var accessToken = _jwtGenerator.GenerateToken(user);
        var newRefresh = await _refreshTokenService.CreateAsync(user.Id);

        await _refreshTokenService.RevokeAsync(request.RefreshToken);

        return Ok(new
        {
            token = accessToken,
            refreshToken = newRefresh.Token,
            user
        });
    }
    
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
    {
        await _refreshTokenService.RevokeAsync(request.RefreshToken);
        return NoContent();
    }
}
