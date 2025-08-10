using System.Security.Claims;
using AnimeApi.Server.Core;
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
    private readonly CookieOptions _cookieOptions = new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Path = "/",
        Domain = null
    };

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
    
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null)
        {
            return Unauthorized();
        }
        
        await _refreshTokenService.RevokeByUserIdAsync(user.Id);
        return NoContent();
    }

    [HttpPost("cookie/google")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GoogleLoginCookie([FromBody] GoogleLoginRequest request)
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
        
        await _refreshTokenService
            .RevokeByUserIdAsync(userDto.Id);
        
        var accessToken = _jwtGenerator.GenerateToken(userDto);
        
        var refreshToken = await _refreshTokenService.CreateAsync(userDto.Id);
        
        SetTokenCookies(accessToken, refreshToken);
        return Ok(new { user = userDto });
    }
    
    [HttpPost("cookie/refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> WebRefresh()
    {
        var refreshTokenFromCookie = Request.Cookies[Constants.Authentication.RefreshTokenCookieName];

        if (string.IsNullOrEmpty(refreshTokenFromCookie))
            return Unauthorized("Refresh token not found.");

        var validation = await _refreshTokenService.ValidateAsync(refreshTokenFromCookie);
        if (!validation.Success)
            return Unauthorized("Invalid or expired refresh token.");

        var user = await _userService.GetByIdAsync(validation.UserId);
        if (user is null)
            return Unauthorized("User not found.");

        var accessToken = _jwtGenerator.GenerateToken(user);
        var newRefresh = await _refreshTokenService.CreateAsync(user.Id);
        await _refreshTokenService.RevokeAsync(refreshTokenFromCookie);

        SetTokenCookies(accessToken, newRefresh);

        return Ok(new { user });
    }

    [HttpPost("cookie/logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> WebLogout()
    {
        var refreshTokenFromCookie = Request.Cookies[Constants.Authentication.RefreshTokenCookieName];

        if (!string.IsNullOrEmpty(refreshTokenFromCookie))
        {
            await _refreshTokenService.RevokeAsync(refreshTokenFromCookie);
        }

        ClearTokenCookies();
        return NoContent();
    }

    private void SetTokenCookies(string accessToken, RefreshTokenResult refreshToken)
    {
        var accessTokenOptions = new CookieOptions
        {
            HttpOnly = _cookieOptions.HttpOnly,
            Secure = _cookieOptions.Secure,
            SameSite = _cookieOptions.SameSite,
            Path = _cookieOptions.Path,
            Domain = _cookieOptions.Domain,
            MaxAge = TimeSpan.FromMinutes(Constants.Authentication.AccessTokenExpirationMinutes)
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = _cookieOptions.HttpOnly,
            Secure = _cookieOptions.Secure,
            SameSite = _cookieOptions.SameSite,
            Path = _cookieOptions.Path,
            Domain = _cookieOptions.Domain,
            Expires = new DateTimeOffset(refreshToken.MetaData.ExpiresAt),
            MaxAge = TimeSpan.FromDays(Constants.Authentication.RefreshTokenExpirationDays)
        };
        
        Response.Cookies
            .Append(Constants.Authentication.AccessTokenCookieName, accessToken, accessTokenOptions);
        
        Response.Cookies
            .Append(Constants.Authentication.RefreshTokenCookieName, refreshToken.Token, refreshTokenOptions);
    }

    private void ClearTokenCookies()
    {
        Response.Cookies.Delete(Constants.Authentication.AccessTokenCookieName);
        Response.Cookies.Delete(Constants.Authentication.RefreshTokenCookieName);
    }
}
