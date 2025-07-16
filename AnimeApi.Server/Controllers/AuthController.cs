using AnimeApi.Server.Business;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using AnimeApi.Server.Core.Objects;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("[Controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtGenerator _jwtGenerator;

    public AuthController(IUserService userService, IJwtGenerator jwtGenerator)
    {
        _userService = userService;
        _jwtGenerator = jwtGenerator;
    }

    [HttpPost("google")]
    [ProducesResponseType(Constant.StatusCode.Ok)]
    [ProducesResponseType(Constant.StatusCode.BadRequest)]
    [ProducesResponseType(Constant.StatusCode.Unauthorized)]
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
        var token = _jwtGenerator.GenerateToken(userDto);

        return Ok(new
        {
            token,
            user = userDto
        });
    }
}
