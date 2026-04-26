using AnimeApi.Server.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Authorize(Policy = Constants.UserAccess.Admin)]
[Route("[controller]")]
public class HealthController(HealthCheckService healthService) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> HealthCheck()
        => Ok(await healthService.CheckHealthAsync());
}