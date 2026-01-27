using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController(ICachingService cache) : Controller
{
    [HttpGet("cache")]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CacheStatus()
        => Ok(cache.GetStatistics());
}