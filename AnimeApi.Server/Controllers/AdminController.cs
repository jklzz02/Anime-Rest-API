using AnimeApi.Server.Core;
using AnimeApi.Server.Core.Abstractions.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeApi.Server.Controllers;

[ApiController]
[Authorize(Policy = Constants.UserAccess.Admin)]
[Route("[controller]")]
public class AdminController : Controller
{
    [HttpDelete("cache/clear")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public Task<IActionResult> ClearCache([FromServices] ICachingService cache)
    {
        try
        {
            cache.Clear();
            return Task.FromResult<IActionResult>(NoContent());
        }
        catch (Exception ex)
        {
            return Task.FromResult<IActionResult>(
                StatusCode(StatusCodes.Status503ServiceUnavailable,
                    new
                    {
                        error = Constants.HttpRemark.Unavailable,
                        details = ex.Message 
                    }));
        }
    }
}