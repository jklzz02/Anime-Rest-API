using AnimeApi.Server.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController(HealthCheckService healthService) : Controller
{
    [HttpGet]
    [Authorize(Policy = Constants.UserAccess.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CacheStatus()
        => Ok(await healthService.CheckHealthAsync());
}