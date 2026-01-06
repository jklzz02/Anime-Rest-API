using System.Security.Claims;
using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Extensions;
using AnimeApi.Server.Core.Objects;
using AnimeApi.Server.Core.Objects.Auth;
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
    private readonly ISocialAuthService _socialAuthService;
    private readonly CookieOptions _cookieOptions;
    

    public AuthController(
        IUserService userService,
        IJwtGenerator jwtGenerator,
        IRefreshTokenService refreshTokenService,
        ISocialAuthService socialAuthService)
    {
        _userService = userService;
        _jwtGenerator = jwtGenerator;
        _refreshTokenService = refreshTokenService;
        _socialAuthService = socialAuthService;
        
        _cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/"
        };
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var result = await
            _socialAuthService.AuthenticateUserAsync(request);

        if (result.IsFailure)
        {
            return Unauthorized(result.Errors.ToKeyValuePairs());
        }
        
        var user = await
            _userService.GetOrCreateUserAsync(result.Data);
        
        await _refreshTokenService.RevokeByUserIdAsync(user.Id);
        
        var accessToken = _jwtGenerator.GenerateToken(user);
        var refreshToken = await _refreshTokenService.CreateAsync(user.Id);

        return Ok(new
        {
            access_token = accessToken,
            refresh_token = refreshToken,
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
        await _refreshTokenService.RevokeByUserIdAsync(user.Id);
        var newRefresh = await _refreshTokenService.CreateAsync(user.Id);

        return Ok(new
        {
            token = accessToken,
            refreshToken = newRefresh.Token,
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

    [HttpPost("cookie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CookieLogin([FromBody] AuthRequest request)
    {
        var result = await
            _socialAuthService.AuthenticateUserAsync(request);
            
        if (result.IsFailure)
        {
            return Unauthorized(result.Errors.ToKeyValuePairs());
        }
        
        var user = await
            _userService
                .GetOrCreateUserAsync(result.Data);

        await _refreshTokenService.RevokeByUserIdAsync(user.Id);

        var accessToken = _jwtGenerator.GenerateToken(user);
        var refreshToken = await _refreshTokenService.CreateAsync(user.Id);

        SetTokenCookies(accessToken, refreshToken);
        
        return Ok(new
        {
            access_token = accessToken
        });
    }

    [HttpPost("cookie/refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> WebRefresh()
    {
        var refreshTokenFromCookie = Request.Cookies[Constants.Auth.RefreshTokenCookieName];

        if (string.IsNullOrEmpty(refreshTokenFromCookie))
            return Unauthorized("Refresh token not found.");

        var validation = await _refreshTokenService.ValidateAsync(refreshTokenFromCookie);
        if (!validation.Success)
            return Unauthorized("Invalid or expired refresh token.");

        var user = await _userService.GetByIdAsync(validation.UserId);
        if (user is null)
            return Unauthorized("User not found.");

        await _refreshTokenService.RevokeByUserIdAsync(user.Id);
        var accessToken = _jwtGenerator.GenerateToken(user);
        var newRefresh = await _refreshTokenService.CreateAsync(user.Id);

        SetTokenCookies(accessToken, newRefresh);

        return Ok(new
        {
            access_token = accessToken
        });
    }

    [Authorize]
    [HttpPost("cookie/logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> WebLogout()
    {
        var email = User.FindFirst(ClaimTypes.Email);
        var user = await _userService.GetByEmailAsync(email?.Value ?? string.Empty);

        if (user is null)
        {
            return Unauthorized("User not found.");
        }

        await _refreshTokenService.RevokeByUserIdAsync(user.Id);

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
            MaxAge = TimeSpan.FromMinutes(Constants.Auth.AccessTokenExpirationMinutes)
        };

        var refreshTokenOptions = new CookieOptions
        {
            HttpOnly = _cookieOptions.HttpOnly,
            Secure = _cookieOptions.Secure,
            SameSite = _cookieOptions.SameSite,
            Path = _cookieOptions.Path,
            MaxAge = TimeSpan.FromDays(Constants.Auth.RefreshTokenExpirationDays)
        };

        Response.Cookies.Append(
            Constants.Auth.AccessTokenCookieName, 
            accessToken, 
            accessTokenOptions);

        Response.Cookies.Append(
            Constants.Auth.RefreshTokenCookieName, 
            refreshToken.Token, 
            refreshTokenOptions);
    }

    private void ClearTokenCookies()
    {
        var deleteOptions = new CookieOptions
        {
            HttpOnly = _cookieOptions.HttpOnly,
            Secure = _cookieOptions.Secure,
            SameSite = _cookieOptions.SameSite,
            Path = _cookieOptions.Path
        };

        Response.Cookies.Delete(Constants.Auth.AccessTokenCookieName, deleteOptions);
        Response.Cookies.Delete(Constants.Auth.RefreshTokenCookieName, deleteOptions);
    }
}